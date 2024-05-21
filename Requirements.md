# Library Management System API Requirements

## Objective
Develop a REST based Api for library management system to automate the process of borrowing and returning books by users.

## Functional

### Users
- register (user)
- login (user)
- Activate/Deactivate (admin)

### Books
- Add (Admin)
- Update(Admin)
- Delete(Admin)
- Search(all)
- getDetails(all)

### Borrow
- getallBooks(all)
- borrowBook
- renewBook
- returnBook (if overdue fine of 5rs/day)
- viewOverDueBooks

### Analytics
- get data of books borrowed / retured based on date range
- get data of overdue books based on date range

### Reserve
- Reserve books(auth)
- Cancel reservations(auth)
- View reservation status(auth)

## Non Functional
- optimization
- security
- scalability