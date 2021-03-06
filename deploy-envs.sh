#!/bin/bash

# set environment variables used in deploy.sh and AWS task-definition.json:
export IMAGE_NAME=netcoreapps-welthperk
export IMAGE_NAME_API=netcoreapps-welthperk-api
export IMAGE_NAME_REACT=netcoreapps-welthperk-react
export IMAGE_VERSION=latest

export AWS_DEFAULT_REGION=us-west-2
export AWS_ECS_CLUSTER_NAME=default
export AWS_VIRTUAL_HOST=ec2-54-71-183-9.us-west-2.compute.amazonaws.com
export AWS_INTERNAL_HOST=ip-172-31-2-234.us-west-2.compute.internal

# set any sensitive information in travis-ci encrypted project settings:
# required: AWS_ACCOUNT_NUMBER, AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY
# optional: SERVICESTACK_LICENSE
