#!/bin/bash

filename="gfs_$(date +"%F %T").zip"
yandexCloud="/media/yandexCloud/backups/"
mailCloud="/media/mailCloud/backups/"
orico="/media/orico/backups_development/"

echo "Create archive $filename"
git archive --output=../backup/"$filename" HEAD

echo "Copy to yandex cloud $yandexCloud"
cp ../backup/"$filename" $yandexCloud

echo "Copy to mail cloud $mailCloud"
cp ../backup/"$filename" $mailCloud

echo "Copy to orico $orico"
cp ../backup/"$filename" $orico

echo "Done"