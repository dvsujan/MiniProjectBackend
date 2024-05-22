# Endpoints

### User Endpoints

1. **Register User**
   - **URL**: `/api/users/register`
   - **Method**: `POST`
   - **Request DTO**:
     ```json
     {
       "email": "string",
       "username": "string",
       "password": "string"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "email": "string",
            "username": "string",
            "active": "bool"
        }
      ```

2. **Login User**
   - **URL**: `/api/users/login`
   - **Method**: `POST`
   - **Request DTO**:
     ```json
     {
       "email": "string",
       "password": "string"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "token": "string"
        }
      ```

3. **Activate/Deactivate User (Admin)**
   - **URL**: `/api/users/status`
   - **Method**: `PATCH`
   - **query Parameters**: `id`
   - **Request DTO**:
     ```json
     {
       "Active": true | false
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "username" : "string",
            "active": "bool"
        }
      ```

### Book Endpoints

1. **Add Book (Admin)**
   - **URL**: `/api/books`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "title": "string",
       "author": "string",
       "publishedDate": "date",
       "genre": "string",
       "copies": "int"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "title": "string",
            "author": "string",
            "publishedDate": "date",
            "genre": "string",
            "copies": "int"
        }
      ```

2. **Update Book (Admin)**
   - **URL**: `/api/books`
   - **Method**: `PUT`
   - **query Parameters**: `id`
   - **DTO**:
     ```json
     {
       "title": "string",
       "author": "string",
       "publishedDate": "date",
       "genre": "string",
       "copies": "int"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "title": "string",
            "author": "string",
            "publishedDate": "date",
            "genre": "string",
            "copies": "int"
        }
      ```

3. **Delete Book (Admin)**
   - **URL**: `/api/books`
   - **Method**: `DELETE`
   - **Query Parameters**: `id`
   - **Response DTO**:
     ```json
     {
       "id": "int",
       "title": "string",
       "author": "string",
       "publishedDate": "date",
       "genre": "string",
       "copies": "int"
     }
     ```

4. **Search Books**
   - **URL**: `/api/books/search`
   - **Method**: `GET`
   - **Query Parameters**: `title`
   - **Response DTO**:
     ```json
     [
        {
            "id": "int",
            "title": "string",
            "author": "string",
            "publishedDate": "date",
            "genre": "string",
            "copies": "int"
        }
     ]
     ``` 

5. **Get Book Details**
   - **URL**: `/api/books`
   - **Method**: `GET`
   - **Query Parameters**: `id`
   - **Response DTO**:
     ```json
     {
       "id": "int",
       "title": "string",
       "author": "string",
       "publishedDate": "date",
       "genre": "string",
       "copies": "int",
     }
     ```

### Borrow Endpoints

1. **Get All Books**
   - **URL**: `/api/books`
   - **Method**: `GET`
   - **Response DTO**:
     ```json
     [
        {
            "id": "int",
            "title": "string",
            "author": "string",
            "publishedDate": "date",
            "genre": "string",
            "copies": "int",
        }
     ]
     ```

2. **Borrow Book**
   - **URL**: `/api/borrow`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "userId": "int",
       "bookId": "int",
       "dueDate": "date"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "userId": "int",
            "bookId": "int",
            "borrowDate": "date",
            "dueDate": "date",
        }
      ```

3. **Renew Book**
   - **URL**: `/api/borrow/renew`
   - **Method**: `PATCH`
   - **Query Parameters**: `id`
   - **DTO**:
     ```json
     {
       "newDueDate": "date"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "userId": "int",
            "bookId": "int",
            "borrowDate": "date",
            "dueDate": "date",
        }
      ```

4. **Return Book**
   - **URL**: `/api/borrow/return`
   - **Method**: `PATCH`
   - **Query Parameters**: `id`
   - **DTO**:
     ```json
     {
       "returnDate": "date"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "book":Book,
            "borrowDate": "date",
            "dueDate": "date",
            "returnDate": "date",
            "fine": "int"
        }
      ```

5. **Get Overdue Books**
   - **URL**: `/api/borrow/overdue`
   - **Method**: `GET`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "book":Book,
            "borrowDate": "date",
            "dueDate": "date",
            "returnDate": "date",
            "fine": "int"
        }
     ]
     ```
6. **Get All Borrowed Books**
   - **URL**: `/api/borrow/borrowed`
   - **Method**: `GET`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "book":Book,
            "borrowDate": "date",
            "dueDate": "date",
            "returnDate": "date",
            "fine": "int"
        }
     ]
     ```

### Analytics Endpoints

1. **Get Borrowed/Returned Books Data**
   - **URL**: `/api/analytics/borrowed-returned`
   - **Method**: `GET`
   - **Query Parameters**: `startDate`, `endDate`
   - **response DTO**:
     ```json
     [
        {
            "borrowed": "int",
            "returned": "int"
        }
     ]
     ```
   

2. **Get Overdue Books Data**
   - **URL**: `/api/analytics/overdue`
   - **Method**: `GET`
   - **Query Parameters**: `startDate`, `endDate`
    -  **response DTO**:
        ```json
        [
            {
                "overdue": "int"
            }
        ]
     ```

### Reservation Endpoints

1. **Reserve Book**
   - **URL**: `/api/reservations`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "userId": "int",
       "bookId": "int"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "reservationDate": "date",
            "status": "string"
        }
      ```

2. **Cancel Reservation**
   - **URL**: `/api/reservations`
   - **Method**: `DELETE`
   - **Query Parameters**: `id`
   - **response DTO**:
     ```json
     {
       "id": "int",
       "reservationDate": "date",
       "status": "string"
     }
     ```

### Payment Endpoints
1. **Pay Fine**
   - **URL**: `/api/payments`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "userId": "int",
       "borrowId": "int",
       "cardId" :"int"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "AmountPaid": "int",
            "paymentDate": "date"
        }
      ```
2. **Get all user cards**
   - **URL**: `/api/payments/cards`
   - **Method**: `GET`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "cardNumber": "string",
            "expiryDate": "date",
            "cvv": "int"
        }
     ]
     ```
3. **Add Card**
   - **URL**: `/api/payments/cards`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "cardNumber": "string",
       "expiryDate": "date",
       "cvv": "int"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "cardNumber": "string",
            "expiryDate": "date",
            "cvv": "int"
        }
      ```

4. **Delete Card**
5. **URL**: `/api/payments/cards`
   - **Method**: `DELETE`
   - **Query Parameters**: `id`
   - **response DTO**:
     ```json
     {
       "id": "int",
       "cardNumber": "string",
       "expiryDate": "date",
       "cvv": "int"
     }
     ```
6. **Get Payment History**
   - **URL**: `/api/payments/history`
   - **Method**: `GET`
   - **Query Parameters**: `userId`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "AmountPaid": "int",
            "paymentDate": "date"
        }
     ]
     ```

### Review Endpoints

1. **Add Review**
   - **URL**: `/api/reviews`
   - **Method**: `POST`
   - **DTO**:
     ```json
     {
       "userId": "int",
       "bookId": "int",
       "rating": "int",
       "comment": "string"
     }
     ```
    - **Response DTO**:
      ```json
        {
            "id": "int",
            "userId": "int",
            "bookId": "int",
            "rating": "int",
            "comment": "string"
        }
      ```
2. **Get Reviews By User**
   - **URL**: `/api/reviews/user`
   - **Method**: `GET`
   - **Query Parameters**: `userId`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "userId": "int",
            "bookId": "int",
            "rating": "int",
            "comment": "string"
        }
     ]
     ```
3. **Get Reviews By Book** 
   - **URL**: `/api/reviews/book`
   - **Method**: `GET`
   - **Query Parameters**: `bookId`
   - **response DTO**:
     ```json
     [
        {
            "id": "int",
            "userId": "int",
            "bookId": "int",
            "rating": "int",
            "comment": "string"
        }
     ]
     ```