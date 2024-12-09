
CREATE DATABASE College1en;
GO 
USE College1en;

CREATE TABLE Programs (
    ProgId VARCHAR(5) NOT NULL,
    ProgName VARCHAR(50) NOT NULL,
    PRIMARY KEY (ProgId)
);


CREATE TABLE Courses (
    CId VARCHAR(7) NOT NULL,
    CName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    PRIMARY KEY (CId),
    FOREIGN KEY (ProgId) REFERENCES Programs(ProgId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);


CREATE TABLE Students (
    StId VARCHAR(10) NOT NULL,
    StName VARCHAR(50) NOT NULL,
    ProgId VARCHAR(5) NOT NULL,
    PRIMARY KEY (StId),
    FOREIGN KEY (ProgId) REFERENCES Programs(ProgId)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
);


CREATE TABLE Enrollments (
    StId VARCHAR(10) NOT NULL,
    CId VARCHAR(7) NOT NULL,
    FinalGrade INT,
    PRIMARY KEY (StId, CId),
    FOREIGN KEY (StId) REFERENCES Students(StId)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    FOREIGN KEY (CId) REFERENCES Courses(CId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);


INSERT INTO Programs(ProgId, ProgName) 
      VALUES ('P0001', 'Computer Science'),
	         ('P0002', 'Cuisine'),
			 ('P0003', 'Engineering'),
	         ('P0004', 'Business');
			 

INSERT INTO Courses (CId, CName, ProgId)
      VALUES ('C000001', 'Data Base I', 'P0001'),
	         ('C000002', 'Financial Account', 'P0004'),
			 ('C000003', 'Strength of Materials', 'P0003'),
			 ('C000004', 'Fluid Mechanics', 'P0003'),
	         ('C000005', 'Web Development', 'P0001'),
			 ('C000006', 'Entrepreneurship', 'P0004'),
			 ('C000007', 'History of Gastronomy', 'P0002');


INSERT INTO Students(StId, StName, ProgId) 
      VALUES ('S000000001', 'Emanuel Flores', 'P0001'),
	         ('S000000002', 'Isabelle Flores', 'P0004'),
			 ('S000000003', 'Jassana Oliveira', 'P0002'),
	         ('S000000004', 'Eslovam Possamai', 'P0003'),
			 ('S000000005', 'Rayane Silva', 'P0004'),
			 ('S000000006', 'Andrea Delattre', 'P0003'),
			 ('S000000007', 'Michelle Zangrande', 'P0004');


INSERT INTO Enrollments(StId, CId, FinalGrade) 
      VALUES ('S000000001', 'C000001', 98),
	         ('S000000002', 'C000002', 95),
			 ('S000000003', 'C000007', 80),
	         ('S000000004', 'C000004', 91),
			 ('S000000005', 'C000006', 88),
			 ('S000000006', 'C000003', 74),
			 ('S000000007', 'C000006', 86);        