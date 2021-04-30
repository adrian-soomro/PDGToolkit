#!/bin/bash
tempHtmlFileName="tmp.html"
outputName="$1-result.png"

# download the html file served by the server atm
wget http://localhost:3000 -O $tempHtmlFileName


htmlFile=$(cat $tempHtmlFileName)

# remove everything before 'base64,'
result=${htmlFile##*base64\,}

# remove everything after '"/>'
result=${result%%\"/>*}

# convert hex '=' to ascii
result="${result//\&\#x3D;/=}"

# decode the base64 string and save it in $outputName
echo "$result"  | base64 --decode > "$outputName"
echo "Successfully saved $outputName"
rm $tempHtmlFileName
