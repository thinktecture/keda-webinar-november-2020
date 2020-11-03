#!/bin/bash

# Variables
RG_NAME=keda-webinar
AZ_REGION=westeurope
AKS_NAME=aks-keda-webinar-2020
# provide globally unique names for ACR and Storage Account
ACR_NAME=kedawebinar2020
# StorageAccount: maxlength:24 no-dashes
STORAGE_ACCOUNT_NAME=sakedawebinar2020

# Create an Azure Resource Group
echo 'Creating Azure Resource Group ' $RG_NAME
az group create -n $RG_NAME -l $AZ_REGION

# Create an Azure Container Registry
echo 'Creating Azure Container Registry ' $ACR_NAME
az acr create -n $ACR_NAME --sku Basic --admin-enabled false -g $RG_NAME -l $AZ_REGION

# Grab ACR identifier
acrid=$(az acr show -n $ACR_NAME --query 'id' -o tsv)

# Create an Azure Kubernetes Service
# ensure you have an SSH Key configured on your system or use proper arguments to create an AKS instance
echo 'Creating Azure Kubernetes Service ' $AKS_NAME
az aks create -n $AKS_NAME -g $RG_NAME -l $AZ_REGION --enable-managed-identity --node-count 2 --attach-acr $acrid

# Create an Azure Storage Account
echo 'Creating Azure Storage Account ' $STORAGE_ACCOUNT_NAME
az storage account create -n $STORAGE_ACCOUNT_NAME -g $RG_NAME -l $AZ_REGION --sku Standard_LRS --default-action Allow
# Create Queues and Blob Containers
az storage queue create -n aaa-tasks --auth-mode login --account-name $STORAGE_ACCOUNT_NAME
az storage queue create -n aaa-transformed --auth-mode login --account-name $STORAGE_ACCOUNT_NAME
az storage container create -n processed --auth-mode login --account-name $STORAGE_ACCOUNT_NAME --public-access off

# Install Kubectl
# echo 'Installing Kubernetes CLI'
# az aks install-cli

# Get Azure Kubernetes Service credentials
echo 'Setting AKS credentials for Kubernetes CLI'
az aks get-credentials -n $AKS_NAME -g $RG_NAME --overwrite-existing

echo -n 'Done'
