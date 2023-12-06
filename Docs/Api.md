# Buber Dinner Api

## Auth

### Register

```js
POST {{host}}/auth/register
```

### Register request

```json
{
  "firstName": "vo",
  "lastName": "thong khoa",
  "email": "khoa123@gmail.com",
  "password": "123456"
}
```

```js
POST {{host}}/auth/login
```

### Login request

```json
{
  "email": "khoa123@gmail.com",
  "password": "123456"
}
```
