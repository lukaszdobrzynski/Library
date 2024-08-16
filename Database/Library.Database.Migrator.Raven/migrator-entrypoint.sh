#!/bin/bash
echo "Adding nodes to cluster..."

sleep 5

addNode2Response=$(curl -s -S -w "%{http_code}" -X PUT http://library-one:8080/admin/cluster/node?url=http://library-two:8081)
addNode2StatusCode="${addNode2Response: -3}"
addNode2ResponseBody="${addNode2Response:0:${#addNode2Response}-3}"

if [ $? -ne 0 ]; then
    echo "Curl command to add node 2 failed."
    exit 1
fi

if [ "$addNode2StatusCode" -eq 204 ]; then
    echo "Node 2 added successfully with server response status code ${addNode2StatusCode}."
else
    echo "Failed to add node 2 with server response status code ${addNode2StatusCode}."
    echo "Server returned error details: $addNode2ResponseBody"
    exit 1
fi

addNode3Response=$(curl -s -S -w "%{http_code}" -X PUT http://library-one:8080/admin/cluster/node?url=http://library-three:8082)
addNode3StatusCode="${addNode3Response: -3}"
addNode3ResponseBody="${addNode3Response:0:${#addNode3Response}-3}"

if [ $? -ne 0 ]; then
    echo "Curl command to add node 2 failed."
    exit 1
fi

if [ "$addNode3StatusCode" -eq 204 ]; then
    echo "Node 3 added successfully with server response status code ${addNode3StatusCode}."
else
    echo "Failed to add node 3 with server response status code ${addNode3StatusCode}."
    echo "Server returned error details: $addNode3ResponseBody"
    exit 1
fi

echo "Creating database..."

sleep 5

createDatabaseResponseStatusCode=$(curl -o NUL -w "%{response_code}" -X PUT -d @init-db.json -H 'Content-Type:application/json' http://library-one:8080/admin/databases)

if [ $? -ne 0 ]; then
    echo "Curl command to create database failed."
    exit 1
fi

if [ "$createDatabaseResponseStatusCode" -eq 201 ]; then
    echo "Database created successfully."
else
    echo "Failed to create database with with server response status code ${createDatabaseResponseStatusCode}."
    exit 1
fi

echo "Seeding database..."

sleep 5

dotnet /migratorapp/Library.Database.Migrator.Raven.dll

if [ $? -ne 0 ]; then
    echo "Failed to seed the database."
    exit 1
fi

echo "Database seeded successfully."

exit 0