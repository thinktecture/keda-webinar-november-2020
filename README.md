# KEDA Webinar

This repository contains the sample application shown as part of my KEDA webinar.

## Requirements

Verify you have installed and configured the following software on your system

- [git](https://git-scm.com)
- [docker](https://docker.com)
- [helm](https://helm.sh)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

## Sample Sources

The sample application consists of three major components.  

The [Message Producer](src/MessageProducer), which is responsible to publish messages to the Azure Storage Account Queue.

The [Message Transformer](src/MessageTransformer) , which is responsible to transform messages from the inbound Azure Storage Account Queue, and publish messages of a different format to another Azure Storage Account Queue.

The [Message Dispatcher](src/MessageDispatcher), which creates and publishes a blob to Azure Storage Account blob storage based on messages created from the _Message Transformer_.

## Questions & Feedback

If you have questions or feedback, create an issue or submit a pull request.
