# Power Diary Chat Report Interface

This application implements a chat report interface where users can view chat history at varying levels of time-based aggregation.
It allows users to see chat events in descending chronological order and aggregates them based on different time granularities.

## Testing

You can run this web application in 3 steps:
1. `docker-compose up db`, then wait unit database will be available
2. `docker-compose up migrator`, then wait until all migration will be completed
3. `docker-compose up webapp`, then go to `localhost:5010` and you can see web UI

You need to choose some date and report granularity and click 'Generate report' button. You will get a table with test data.

## Tech Info

The project uses clean architecture with Domain, Data and Web layers.

Also the project contains some unit tests which checking logic in the memory.
