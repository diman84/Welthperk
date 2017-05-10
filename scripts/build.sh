#!/bin/bash
source ../deploy-envs.sh

#AWS_ACCOUNT_NUMBER={} set in private variable
export AWS_ECS_REPO_DOMAIN=$AWS_ACCOUNT_NUMBER.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com

# Build process
docker build -t $IMAGE_NAME_API ../api
docker tag $IMAGE_NAME_API $AWS_ECS_REPO_DOMAIN/$IMAGE_NAME_API:$IMAGE_VERSION

cp ../package.json ../client-react
docker build -t $IMAGE_NAME_REACT ../client-react
docker tag $IMAGE_NAME_REACT $AWS_ECS_REPO_DOMAIN/$IMAGE_NAME_REACT:$IMAGE_VERSION
