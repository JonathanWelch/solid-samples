# solid-samples
Examples of code not adhering to the SOLID Principles

## 1-SingleResponsibility-WebApi
The StockController class in Controllers folder has too much responsibility.

## 2.0-OpenClosed
The MDAService class has the violation.  
The method CommitChanges(IEnumerable<MDA.MDA> mdaList) includes flow control if statements on logic to execute based on the type of the IEvent.  So if a new concrete implmentation of IEvent type was added this code would need to be updated.    

## 2.1-OpenClosed
The MatfloAdapter class has the violation.
The method GetOperationName(StockRequestType stockRequestType) has a switch statement to return a string based on what the StockRequestType is.  So if a new StockRequestType was added this class would need to be amended as otherwise a NotImplementedException would be thrown.

## 5.0-DependencyInversion
The ListerHillsStockCheck class has the violation.
The method ExecuteJob(IJobExecutionContext context) uses the static DateTime.UtcNow property.


## 5.1-DependencyInversion
ListerHillsStockFileProcessor class has the violation
The method Process(FileInfo inputFile) news up a CsvFileReader.  Could mention the CommandAndQueryExecutor which also news up a query class, which you cant pass in.
 
