#!/bin/sh
for i in $(env | grep NAVTRACK_)
do
    key=$(echo $i | cut -d '=' -f 1)
    value=$(echo $i | cut -d '=' -f 2-)
    echo $key=$value
 
    find /app -type f -name '*.js' -exec sed -i "s|${key}|${value}|g" '{}' +
done

serve /app
