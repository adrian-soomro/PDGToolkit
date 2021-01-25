#!/bin/bash
timeout=${SETUP_TIMEOUT:-600}
SWAGGERUI="http://localhost:1337/swagger/index.html"
SWAGGERFILE="http://localhost:1337/swagger/v1/swagger.json"

echo "Starting Schema UI Server"

nohup dotnet run --project ./src/PDGToolkitSchemaUI/PDGToolkitSchemaUI.csproj &

echo "Waiting for $SWAGGERUI to be ready..."

n=0
until [ $n -ge $timeout ]; do
  httpStatusCode=$(curl -I -s --insecure $SWAGGERUI | cat | head -n 1 | cut -d" " -f2)

  if [ "$httpStatusCode" = "200" ]; then echo "$SWAGGERUI was ready in $n seconds" && break; fi
  n=$((n+1)) 
  sleep 1
done

if [ "$n" -eq "$timeout" ]; then echo "$SWAGGERUI is not ready after $n seconds" && exit 1; fi

mkdir tmp && cd tmp

# to grab the file
wget $SWAGGERFILE -O ./swaggerfile.json

# install widdershins
npm install widdershins

# generate md file
./node_modules/widdershins/widdershins.js --search false --summary ./swaggerfile.json -o ./schema_doc.md

# trim generated file 
sed -n '/# Schemas/,$p' ./schema_doc.md  | sed '1d' > ./temp_schema_doc.md 

# remove everything after a keyword and put it in a temp file
sed -n '/# Schema overview/q;p' ../README.md > ./temp_readme.md

# insert keyword back in 
echo "# Schema overview" >> ./temp_readme.md

# append trimmed generated file to the end of temp file
cat ./temp_schema_doc.md >> ./temp_readme.md

# remove old readme and replace it by the temp file
rm ../README.md && mv ./temp_readme.md ../README.md

# cleanup 
cd .. && rm ./tmp -rf
