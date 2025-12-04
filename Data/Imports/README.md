# CSV Import Folder Structure

This folder contains CSV files for data imports into the Billing File API.

## Folder Structure

```
Data/Imports/
├── pending/       - Place CSV files here to be imported
├── processed/     - Successfully imported files are moved here
└── failed/        - Failed imports are moved here with error logs
```

## Workflow

1. **Upload**: Place your CSV file in the `pending/` folder
2. **Process**: The import service will process the file
3. **Archive**: 
   - On success → moved to `processed/` with timestamp
   - On failure → moved to `failed/` with error log

## File Naming Convention

- Upload as: `your-data.csv`
- After processing: `your-data_YYYYMMDD_HHmmss_processed.csv`
- After failure: `your-data_YYYYMMDD_HHmmss_failed.csv`

## CSV Format Requirements

- **Encoding**: UTF-8
- **Delimiter**: Comma (,)
- **Header**: First row must contain column names
- **Max Size**: 10MB (configurable)

## Supported Import Types

- Hotels
- Reservations
- (Add more as needed)

## Notes

- Empty files are skipped
- Duplicate records are handled by the import logic
- All imports are logged in the application logs
- Failed rows are logged with specific error messages

