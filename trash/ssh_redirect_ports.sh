#!/bin/bash

ssh -NL 5000:127.0.0.1:5000 orangepi@192.168.1.11
ssh -NL 8001:127.0.0.1:8001 orangepi@192.168.1.11