use polideportivo;

CREATE TABLE tblENTRENADORES (
    entID_pk INT PRIMARY KEY,
    entNOMBRE VARCHAR(50),
    entAPELLIDO VARCHAR(50)
);

CREATE TABLE tblTELEFONO_ENTRENADOR (
    tel_entrenadorID_pk INT PRIMARY KEY,
    tel_entrenador VARCHAR(20),
    entID_fk INT,
    FOREIGN KEY (entID_fk) REFERENCES tblENTRENADORES(entID_pk)
);

CREATE TABLE tblCORREO_ENTRENADOR (
    cor_entrenadorID_pk INT PRIMARY KEY,
    Cor_entrenador VARCHAR(100),
    entID_fk INT,
    FOREIGN KEY (entID_fk) REFERENCES tblENTRENADORES(entID_pk)
);

SELECT 
    e.entID_pk as 'id_entrenador',
    e.entNOMBRE as 'nombre',
    e.entAPELLIDO as 'apellido',
    t.tel_entrenador as 'telefono',
    c.Cor_entrenador as 'correo'
FROM tblENTRENADORES e
LEFT JOIN tblTELEFONO_ENTRENADOR t ON e.entID_pk = t.entID_fk
LEFT JOIN tblCORREO_ENTRENADOR c ON e.entID_pk = c.entID_fk;


