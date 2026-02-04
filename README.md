# Hospital Management System – CRUD Operations

## Project Type
Web Application (Frontend: Angular, Backend: .NET Core / ASP.NET Web API, Database: SQL Server)

## Project Description
The Hospital Management System is designed to streamline hospital operations, manage patient data efficiently, and simplify interactions between patients, doctors, and hospital infrastructure.  
It supports CRUD (Create, Read, Update, Delete) operations for all major hospital entities, ensuring accurate record-keeping and optimized workflow management.

## Key Features

### Patient Management
- Add, view, update, and delete patient records.
- Maintain detailed patient profiles including personal, medical, and contact information.

### Doctor Management
- Register and manage doctor information.
- Assign doctors to patient admissions for tracking care.

### Patient Admission Management
- Record patient admissions linked to doctors.
- Track patient admission history, treatments, and discharge details.

### Room and Bed Management
- Manage hospital rooms and bed availability.
- Assign patients to rooms and beds efficiently.

### Relationship Mapping
- Each patient can have multiple admissions.
- Each admission is associated with a specific doctor.
- Each patient is assigned to a room and a bed, enabling proper resource allocation.

## Entity Relationship Overview
- **Patient → PatientAdmission → Doctor**  
- **Patient → Room → Bed**

## Technologies Used
- **Backend:** .NET Core / ASP.NET Web API (for handling CRUD operations, business logic, and API endpoints)  
- **Frontend:** Angular (for building a responsive and interactive user interface)  
- **Database:** SQL Server / MySQL / PostgreSQL (for storing patient, doctor, room, and admission data)  
- **Tools & IDEs:**  
  - Visual Studio / Visual Studio Code  
  - Postman (for testing APIs)  
  - Git / GitHub (for version control)  
  - Angular CLI (for frontend project management)  

## Advantages
- Efficient hospital workflow management.  
- Real-time tracking of patient admissions and bed occupancy.  
- Simplified doctor-patient assignment and record retrieval.  
- Scalable design for future integration with billing, pharmacy, or laboratory modules.


![image alt]( https://github.com/Mantu0-Vineforce-it/Hospital_Crud/blob/c83bfed16e3198e01e57b272a82d9cce8545ddb2/Screenshot%202026-02-03%20143455.png  )
![image alt]( https://github.com/Mantu0-Vineforce-it/Hospital_Crud/blob/c83bfed16e3198e01e57b272a82d9cce8545ddb2/Screenshot%202026-02-03%20143606.png )
  
