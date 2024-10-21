CREATE TABLE Category(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE Supplier(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    contact_info VARCHAR(100),
    address VARCHAR(100)
);

CREATE TABLE Product(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    picture VARCHAR(50),
    description VARCHAR(100),
    category_id VARCHAR(8),
    supplier_id VARCHAR(8),
    FOREIGN KEY (category_id) REFERENCES Category(id),
    FOREIGN KEY (supplier_id) REFERENCES Supplier(id)
);

CREATE TABLE Product_Item(
    id VARCHAR(8) PRIMARY KEY,
    product_id VARCHAR(8),
    quantity INT,
    import_price INT,
    selling_price INT,
    discount DECIMAL(10,2),
    FOREIGN KEY (product_id) REFERENCES Product(id)
);

CREATE TABLE Variation(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE Variation_Option(
    id VARCHAR(8) PRIMARY KEY,
    variation_id VARCHAR(8),
    value VARCHAR(50) NOT NULL,
    FOREIGN KEY (variation_id) REFERENCES Variation(id)
);

CREATE TABLE Product_Configuration(
    product_item_id VARCHAR(8),
    variation_option_id VARCHAR(8),
    PRIMARY KEY (product_item_id, variation_option_id),
    FOREIGN KEY (product_item_id) REFERENCES Product_Item(id),
    FOREIGN KEY (variation_option_id) REFERENCES Variation_Option(id)
);

CREATE TABLE Role_Name(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE Account_State(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE Account(
    id VARCHAR(8) PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(32) NOT NULL,
    name VARCHAR(50),
    phone VARCHAR(20),
    email VARCHAR(50),
    address VARCHAR(100),
    role_id VARCHAR(8),
    state_id VARCHAR(8),
    FOREIGN KEY (role_id) REFERENCES Role_Name(id),
    FOREIGN KEY (state_id) REFERENCES Account_State(id)
);

CREATE TABLE Order_State(
    id VARCHAR(8) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE `Order`(
    id VARCHAR(8) PRIMARY KEY,
    user_id VARCHAR(8),
    date DATETIME NOT NULL,
    state_id VARCHAR(8),
    FOREIGN KEY (user_id) REFERENCES Account(id),
    FOREIGN KEY (state_id) REFERENCES Order_State(id)
);

CREATE TABLE Order_Item(
    id VARCHAR(8) PRIMARY KEY,
    order_id VARCHAR(8),
    product_item_id VARCHAR(8),
    quantity INT NOT NULL,
    price INT NOT NULL,
    FOREIGN KEY (order_id) REFERENCES `Order`(id),
    FOREIGN KEY (product_item_id) REFERENCES Product_Item(id)
);