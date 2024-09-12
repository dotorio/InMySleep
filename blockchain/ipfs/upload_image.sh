#!/bin/bash

directory="./images"

files=($(find "$directory" -type f))

url="http://localhost:9094/add"

upload_failed=false

for file in "${files[@]}"; do
    echo "Uploading $file..."
    curl -F file=@"$file" $url

    if [ $? -ne 0 ]; then
        echo "Failed to upload $file"
        upload_failed=true
    fi
done

if [ $upload_failed = false ]; then
    echo "All files uploaded successfully"
    exit 0
fi