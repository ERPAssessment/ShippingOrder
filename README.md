# ShippingOrder

This repository implements the Shipping Order (SHO) bounded context of a small ERP system, focusing on the fulfillment of Purchase Orders (POs). It follows Domain-Driven Design (DDD) principles and is part of an assessment task.

## Overview
- **Bounded Context**: Shipping Orders (SHOs)  
- **Purpose**: Manage the creation and closure of SHOs, referencing POs and notifying the PurchasingOrder context of state changes.  
- **Technology Stack**: .NET, EF Core, Microsoft SQL Server, Docker  
- **Single Branch**: `main` (all work, including incomplete, will reside here)  

## Entities
- **ShippingOrder**  
  - Number (unique, distinct scheme from POs)  
  - DeliveryDate  
  - PalletCount  
  - Items (list of ShippingOrderItem, matching the referenced PO)  
  - ReferencedPONumber  
- **ShippingOrderItem**  
  - SerialNumber  
  - GoodCode  
  - Price  

## Requirements
- Standalone process, containerized with Docker.  
- Owns its data, stored in a Microsoft SQL Server database.  
- Sends reactive notifications to the PurchasingOrder context when an SHO is created or closed.  

## Setup Instructions
1. Clone the repository: `git clone https://github.com/ERPAssessment/ShippingOrder.git`  
2. Navigate to the directory: `cd ShippingOrder`  
3. Further setup (Docker, database, etc.) will be added as implementation progresses.
