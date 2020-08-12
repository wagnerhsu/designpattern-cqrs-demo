## CQRS [>>](https://codewithshadman.com/cqrs/)
- CRUD-based thinking is when people try to fit all operations with an object into a narrow box of create, read, update, and delete operations. The CRUD-based interface is the result of CRUD-based thinking.

= The Task-based interface is the opposite of the CRUD-based interface. It is the result of identifying each task the user can accomplish with an object and assigning a separate window to each of them. This also affects both the UI and the API
- Any operation can be either a command or a query:
  - A query doesn’t mutate the external state, such as that of the database, but returns something to the caller.
  - A command is the opposite of that. It does mutate the external state but doesn’t return anything to the client.