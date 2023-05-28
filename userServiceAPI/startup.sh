#/usr/bin/bash
export connectionString="mongodb://localhost:27017/"
export userCol="userCol"
export database="Auction"
dotnet run server="$server" port="$port" userCol=$userCol database="$database"