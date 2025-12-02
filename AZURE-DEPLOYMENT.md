# Azure Deployment Guide

This guide provides comprehensive instructions for deploying the Billing File API to Microsoft Azure.

## Azure Services Required

### 1. **Azure App Service** (Web API Hosting)
- **Purpose**: Host the ASP.NET Core Web API
- **Tier Recommendation**: 
  - Development: B1 (Basic)
  - Production: S1 (Standard) or P1V2 (Premium)
- **Features**: Auto-scaling, SSL, deployment slots, custom domains

### 2. **Azure SQL Database**
- **Purpose**: Store billing records
- **Tier Recommendation**:
  - Development: Basic (5 DTU)
  - Production: Standard S2+ or Premium
- **Features**: Automatic backups, geo-replication, threat detection

### 3. **Azure Key Vault** (Recommended)
- **Purpose**: Secure storage of connection strings and secrets
- **Tier**: Standard
- **Features**: Secret management, access policies, audit logging

### 4. **Application Insights** (Recommended)
- **Purpose**: Application monitoring and telemetry
- **Features**: Real-time monitoring, application maps, alerting

### 5. **Azure API Management** (Optional)
- **Purpose**: API gateway, rate limiting, versioning
- **Tier**: Developer or Standard
- **Features**: API policies, throttling, caching, security

## Architecture Diagram

```
Internet
    ↓
[Azure API Management] (Optional)
    ↓
[Azure App Service]
    ├→ [Application Insights]
    ├→ [Azure Key Vault]
    └→ [Azure SQL Database]
```

## Cost Estimation (Monthly)

| Service | Tier | Estimated Cost |
|---------|------|----------------|
| App Service | B1 Basic | ~$13 USD |
| SQL Database | Basic 5 DTU | ~$5 USD |
| Key Vault | Standard | ~$0.03 per 10k ops |
| App Insights | | ~Free tier available |
| **Total Development** | | **~$20-25 USD/month** |
| | | |
| App Service | S1 Standard | ~$70 USD |
| SQL Database | S2 Standard | ~$150 USD |
| **Total Production** | | **~$220+ USD/month** |

## Deployment Steps

### Method 1: Azure Portal (Quick Start)

#### Step 1: Create Azure SQL Database

```bash
# Set variables
RESOURCE_GROUP="rg-billingfile-prod"
LOCATION="eastus"
SQL_SERVER="sql-billingfile-prod"
SQL_DB="BillingFileDb"
SQL_ADMIN="sqladmin"
SQL_PASSWORD="YourSecurePassword123!"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create SQL Server
az sql server create \
  --name $SQL_SERVER \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --admin-user $SQL_ADMIN \
  --admin-password $SQL_PASSWORD

# Configure firewall (allow Azure services)
az sql server firewall-rule create \
  --resource-group $RESOURCE_GROUP \
  --server $SQL_SERVER \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Create database
az sql db create \
  --resource-group $RESOURCE_GROUP \
  --server $SQL_SERVER \
  --name $SQL_DB \
  --service-objective Basic \
  --backup-storage-redundancy Local
```

**Connection String Format:**
```
Server=tcp:sql-billingfile-prod.database.windows.net,1433;Initial Catalog=BillingFileDb;Persist Security Info=False;User ID=sqladmin;Password=YourSecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

#### Step 2: Create App Service

```bash
APP_SERVICE_PLAN="asp-billingfile-prod"
WEB_APP="app-billingfile-prod"

# Create App Service Plan
az appservice plan create \
  --name $APP_SERVICE_PLAN \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --plan $APP_SERVICE_PLAN \
  --runtime "DOTNET|8.0"
```

#### Step 3: Create Key Vault

```bash
KEY_VAULT="kv-billingfile-prod"

# Create Key Vault
az keyvault create \
  --name $KEY_VAULT \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION

# Store connection string in Key Vault
az keyvault secret set \
  --vault-name $KEY_VAULT \
  --name "SqlConnectionString" \
  --value "Server=tcp:sql-billingfile-prod.database.windows.net,1433;Initial Catalog=BillingFileDb;..."
```

#### Step 4: Configure App Service with Key Vault

```bash
# Enable Managed Identity for App Service
az webapp identity assign \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP

# Get the principal ID
PRINCIPAL_ID=$(az webapp identity show \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --query principalId -o tsv)

# Grant App Service access to Key Vault
az keyvault set-policy \
  --name $KEY_VAULT \
  --object-id $PRINCIPAL_ID \
  --secret-permissions get list
```

#### Step 5: Configure Application Insights

```bash
# Create Application Insights
az monitor app-insights component create \
  --app $WEB_APP \
  --location $LOCATION \
  --resource-group $RESOURCE_GROUP

# Get instrumentation key
INSTRUMENTATION_KEY=$(az monitor app-insights component show \
  --app $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --query instrumentationKey -o tsv)

# Configure App Service with App Insights
az webapp config appsettings set \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --settings APPINSIGHTS_INSTRUMENTATIONKEY=$INSTRUMENTATION_KEY
```

#### Step 6: Deploy Application

##### Option A: Deploy from Visual Studio

1. Right-click on `BillingFile.API` project
2. Select **Publish**
3. Choose **Azure**
4. Select your **App Service**
5. Click **Publish**

##### Option B: Deploy using Azure CLI

```bash
# Build and publish
cd src/BillingFile.API
dotnet publish -c Release -o ./publish

# Create deployment package
cd publish
zip -r ../deploy.zip .

# Deploy to App Service
az webapp deployment source config-zip \
  --resource-group $RESOURCE_GROUP \
  --name $WEB_APP \
  --src ../deploy.zip
```

##### Option C: Deploy using GitHub Actions

Create `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '8.0.x'
    
    - name: Build
      run: dotnet build --configuration Release
      
    - name: Publish
      run: dotnet publish src/BillingFile.API/BillingFile.API.csproj -c Release -o ./publish
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'app-billingfile-prod'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

#### Step 7: Run Database Migrations

After deployment, you need to run EF Core migrations:

##### Option A: From Local Machine

```bash
# Update connection string in appsettings.json with Azure SQL
dotnet ef database update --project src/BillingFile.Infrastructure --startup-project src/BillingFile.API
```

##### Option B: Using Azure CLI (Run Command)

```bash
az webapp ssh --name $WEB_APP --resource-group $RESOURCE_GROUP
cd /home/site/wwwroot
dotnet BillingFile.API.dll ef database update
```

##### Option C: Use Startup Migration (Already implemented)

The application automatically runs migrations on startup in Development mode. For production:

Update `Program.cs` to run migrations:

```csharp
// In production, uncomment this to run migrations automatically
if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
```

### Method 2: Using Terraform (Infrastructure as Code)

Create `main.tf`:

```hcl
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-billingfile-prod"
  location = "East US"
}

resource "azurerm_sql_server" "sql" {
  name                         = "sql-billingfile-prod"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "sqladmin"
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_sql_database" "db" {
  name                = "BillingFileDb"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  server_name         = azurerm_sql_server.sql.name
  edition             = "Basic"
}

resource "azurerm_app_service_plan" "asp" {
  name                = "asp-billingfile-prod"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_app_service" "app" {
  name                = "app-billingfile-prod"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id

  site_config {
    linux_fx_version = "DOTNETCORE|8.0"
    always_on        = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT" = "Production"
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "SQLAzure"
    value = "Server=tcp:${azurerm_sql_server.sql.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.db.name};..."
  }
}
```

Deploy with Terraform:

```bash
terraform init
terraform plan
terraform apply
```

## Post-Deployment Configuration

### 1. Configure CORS (if needed)

```bash
az webapp cors add \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --allowed-origins "https://yourdomain.com"
```

### 2. Enable HTTPS Only

```bash
az webapp update \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --https-only true
```

### 3. Configure Custom Domain (Optional)

```bash
az webapp config hostname add \
  --webapp-name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --hostname "api.yourdomain.com"
```

### 4. Set Up Deployment Slots (Production)

```bash
# Create staging slot
az webapp deployment slot create \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --slot staging

# Deploy to staging first, then swap
az webapp deployment slot swap \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP \
  --slot staging
```

## Monitoring and Maintenance

### Application Insights Queries

```kusto
// Failed requests
requests
| where success == false
| order by timestamp desc

// Response times
requests
| summarize avg(duration) by bin(timestamp, 5m)

// Exceptions
exceptions
| order by timestamp desc
```

### Set Up Alerts

```bash
# Create alert for failed requests
az monitor metrics alert create \
  --name "High Failed Requests" \
  --resource-group $RESOURCE_GROUP \
  --scopes /subscriptions/{subscription-id}/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.Web/sites/$WEB_APP \
  --condition "count requests/failed > 10" \
  --window-size 5m
```

## Security Best Practices

1. **Never commit connection strings** to source control
2. **Use Azure Key Vault** for all secrets
3. **Enable Managed Identity** for Azure service authentication
4. **Configure firewall rules** on SQL Server
5. **Use HTTPS only** for all endpoints
6. **Enable Application Insights** for security monitoring
7. **Implement API rate limiting** with Azure API Management
8. **Regular security updates** for packages and runtime
9. **Use Azure AD authentication** for administrative access
10. **Enable SQL auditing and threat detection**

## Backup and Disaster Recovery

### SQL Database Backups

```bash
# Automated backups are enabled by default
# Configure long-term retention
az sql db ltr-policy set \
  --resource-group $RESOURCE_GROUP \
  --server $SQL_SERVER \
  --database $SQL_DB \
  --weekly-retention P4W \
  --monthly-retention P12M
```

### App Service Backups

```bash
# Create storage account for backups
az storage account create \
  --name stbillingfilebackup \
  --resource-group $RESOURCE_GROUP

# Configure backup
az webapp config backup create \
  --resource-group $RESOURCE_GROUP \
  --webapp-name $WEB_APP \
  --backup-name DailyBackup \
  --storage-account-url "{sas-url}"
```

## Troubleshooting

### View Application Logs

```bash
az webapp log tail \
  --name $WEB_APP \
  --resource-group $RESOURCE_GROUP
```

### Access Kudu Console

Navigate to: `https://app-billingfile-prod.scm.azurewebsites.net`

### Database Connection Issues

1. Check firewall rules on SQL Server
2. Verify connection string in Key Vault
3. Ensure Managed Identity has Key Vault access
4. Check Application Insights for detailed errors

## Scaling

### Horizontal Scaling (Multiple Instances)

```bash
az appservice plan update \
  --name $APP_SERVICE_PLAN \
  --resource-group $RESOURCE_GROUP \
  --number-of-workers 3
```

### Auto-scaling Rules

```bash
az monitor autoscale create \
  --resource-group $RESOURCE_GROUP \
  --resource $APP_SERVICE_PLAN \
  --resource-type Microsoft.Web/serverfarms \
  --name autoscale-billingfile \
  --min-count 1 \
  --max-count 5 \
  --count 1
```

## Cost Optimization

1. Use **Basic tier** for development environments
2. Enable **auto-scaling** to scale down during low usage
3. Use **reserved instances** for predictable workloads (up to 72% savings)
4. Monitor usage with **Azure Cost Management**
5. Set up **budget alerts**
6. Use **deployment slots** instead of separate environments

## Support

For Azure-specific issues:
- Azure Portal: https://portal.azure.com
- Azure Support: https://azure.microsoft.com/support
- Documentation: https://docs.microsoft.com/azure

For application issues:
- Check Application Insights
- Review application logs
- Contact development team

