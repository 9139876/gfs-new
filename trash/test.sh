#!/bin/bash

# if ! [ $(id -u) = 0 ]; then
#    echo "The script need to be run as root." >&2
#    exit 1
# fi

# docker save "$1" | sudo -u insider docker --context orangepi load


sudo docker save "$1" | docker --context orangepi load
