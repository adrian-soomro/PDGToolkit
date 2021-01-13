#!/bin/bash
timeout=${SETUP_TIMEOUT:-600}
SWAGGERUI="http://localhost:1337/swagger/index.html"
SWAGGERFILE="http://localhost:1337/swagger/v1/swagger.json"

echo "Starting Schema UI Server"

nohup dotnet run --project ./src/PDGToolkitUI/PDGToolkitUI.csproj &

echo "Waiting for $SWAGGERUI to be ready..."

n=0
until [ $n -ge $timeout ]; do
  httpStatusCode=$(curl -I -s --insecure $SWAGGERUI | cat | head -n 1 | cut -d" " -f2)

  if [ "$httpStatusCode" = "200" ]; then echo "$SWAGGERUI was ready in $n seconds" && break; fi
  n=$((n+1)) 
  sleep 1
done

if [ "$n" -eq "$timeout" ]; then echo "$SWAGGERUI is not ready after $n seconds" && exit 1; fi

# to grab the file
mkdir tmp && wget $SWAGGERFILE -O ./tmp/swaggerfile.json

#install widdershins
npm install -g widdershins

# generate md file
widdershins --search false --summary ./tmp/swaggerfile.json -o ./tmp/schema_doc.md

# trim generated file
awk '/# Schemas/{p=1}p' ./tmp/schema_doc.md > ./tmp/temp_schema_doc.md 

# remove everything after a keyword and put it in a temp file
sed -n '/# Schemas/q;p' ./README.md > ./tmp/temp_readme.md

# append trimmed generated file to the end of temp file
cat ./tmp/temp_schema_doc.md >> ./tmp/temp_readme.md

# remove old readme and replace it by the temp file
rm ./README.md && mv ./tmp/temp_readme.md ./README.md

# debug - temporary
cat ./README.md

# cleanup 
rm ./tmp -rf

#commit readme.md & push