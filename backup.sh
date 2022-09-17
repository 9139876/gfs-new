#!/bin/bash

filename="gfs_$(date +"%F %T").zip"

git archive --output=../backup/"$filename" HEAD

cp ../backup/"$filename" /media/yandexCloud/backups/
cp ../backup/"$filename" /media/mailCloud/backups/
cp ../backup/"$filename" /media/orico/backups_development/



