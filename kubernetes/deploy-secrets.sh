#!/bin/bash

# Configure variables according to your environment
RG_NAME=keda-webinar
STORAGE_ACCOUNT_NAME=sakedawebinar2020

echo 'Reading Connection String from Azure Storage Account'
pcs=$(az storage account show-connection-string -n $STORAGE_ACCOUNT_NAME -g $RG_NAME -o tsv)

echo 'Creating Kubernetes Secret in namespace message-transformer'
kubectl create secret generic az-storage-auth --namespace message-transformer --from-literal=KedaWebinarStorageAccount="$pcs"

echo 'Creating Kubernetes Secret in namespace message-dispatcher'
kubectl create secret generic az-storage-auth --namespace message-dispatcher --from-literal=KedaWebinarStorageAccount="$pcs" 

echo 'Done.'
