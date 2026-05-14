-- create the comics table in the comics db
CREATE TABLE Comics (
    Id INTEGER PRIMARY KEY, 
    Name TEXT, 
    Year INTEGER, 
    Description TEXT, 
    Price DECIMAL(10,2), 
    ImageFileName TEXT, 
    InWishlist BOOLEAN,
    InCollection BOOLEAN);