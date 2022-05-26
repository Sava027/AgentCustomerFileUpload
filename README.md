# AgentCustomerFileUpload

The main API allows a user to upload multiple files for a customer. A tracking identifier is returned if the files could be saved into a temporary folder. Another method is called for processing the files and saving them into the final storage location (*mocked*).
The basic setup to send an email when the processing of files is complete. This is a POC project and more is work left to make it a production ready.

## Outstanding Considerations:
* connect to cloud content storage (Azure Blobstorage)
* create back end service to process files (Azure functions with Servicebus or similar, this will handle security checks and duplication checks)
* finalize organisational folder structure in the Domain
* add more unit tests
* seperate out more logic into micro services
* deciding on logging
* adding FluentValidation to give consistent error handling 
* add docker compose
