#!/bin/bash

directory="./images"

files=()

while IFS= read -r -d '' file; do
	files+=("$file")
done < <(find "$directory" -type f -print0)

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
