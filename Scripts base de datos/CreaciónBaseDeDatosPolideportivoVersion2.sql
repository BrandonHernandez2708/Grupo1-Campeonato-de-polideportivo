CREATE DATABASE Polideportivo;
USE Polideportivo;

-- 1. Tablas sin claves foráneas
CREATE TABLE tblDEPORTE (
    depID_pk INT PRIMARY KEY,
    DEPNOMBRE VARCHAR(100),
    DEPCANTIDAD_JUGADORES_EQUIPO INT,
    DEPCANTIDAD_JUGADORES_EN_EL_CAMPO INT,
    DEPCANTIDAD_DE_TIEMPOSDEP INT,
    DEPDURACION_DE_CADA_TIEMPO INT,
    DEPDURACION_TOTAL_DEL_PARTIDO INT
);

CREATE TABLE tblCAMPEONATOS (
    camID_pk INT PRIMARY KEY,
    CamNombre VARCHAR(100),
    camMODALIDAD VARCHAR(50),
    camCANT_EQUIPOS INT,
    camFECHA_INICIO DATE,
    camFECHA_FINAL DATE
);

CREATE TABLE tblENTRENADORES (
    entID_pk INT PRIMARY KEY,
    entNOMBRE VARCHAR(50),
    entAPELLIDO VARCHAR(50)
);

CREATE TABLE tblPUESTOS (
    pueID_pk INT PRIMARY KEY,
    pueNombre VARCHAR(50),
    pueDESCRIPCION TEXT
);

CREATE TABLE tblESTADO_PARTIDO (
    ID_ESTADO INT PRIMARY KEY AUTO_INCREMENT,
    DESCRIPC VARCHAR(100)
);

CREATE TABLE tblCANCHA (
    canID INT PRIMARY KEY AUTO_INCREMENT,
    canCapacidad INT,
    canDIRECCION VARCHAR(255)
);

-- 2. Tablas que dependen solo de las anteriores
CREATE TABLE tblEMPLEADOS (
    empDPI_pk BIGINT PRIMARY KEY,
    empNombre VARCHAR(50),
    empAPELLIDO VARCHAR(50),
    fk_id_usuario INT,
    FOREIGN KEY (fk_id_usuario) REFERENCES tbl_usuario(pk_id_usuario)
);

CREATE TABLE tblEQUIPOS (
    equID_pk INT PRIMARY KEY,
    equiNombre VARCHAR(100),
    depID_fk INT,
    equicantidad_integrantes INT,
    entID_fk INT,
    equTELEFONO VARCHAR(20),
    equCORREO VARCHAR(100),
    FOREIGN KEY (depID_fk) REFERENCES tblDEPORTE(depID_pk),
    FOREIGN KEY (entID_fk) REFERENCES tblENTRENADORES(entID_pk)
);

-- 3. Tablas que dependen de empleados, entrenadores o equipos
CREATE TABLE tblTELEFONO_EMPLEADOS (
    Tel_empleado_ID_pk INT PRIMARY KEY,
    tel_empleado VARCHAR(20),
    empDPI_fk BIGINT,
    FOREIGN KEY (empDPI_fk) REFERENCES tblEMPLEADOS(empDPI_pk)
);

CREATE TABLE tblCORREO_EMPLEADOS (
    ID_CORREO_EMP INT PRIMARY KEY,
    CORREO VARCHAR(100),
    empDPI_fk BIGINT,
    FOREIGN KEY (empDPI_fk) REFERENCES tblEMPLEADOS(empDPI_pk)
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

CREATE TABLE tblJUGADORES (
    jugID_PK INT PRIMARY KEY,
    jugNombre VARCHAR(50),
    jugApellido VARCHAR(50),
    jugPOSICION VARCHAR(50),
    equiID_fk INT,
    FOREIGN KEY (equiID_fk) REFERENCES tblEQUIPOS(equID_pk)
);

CREATE TABLE tblCONTRATACION_CAMBIO_PUESTO (
    pueID_pk_fk INT,
    empDPI_fk BIGINT,
    conFECHA_DE_CAMBIO DATE,
    conTipo_Operacion TINYINT, -- 1 = contratación, 0 = cambio de puesto
    PRIMARY KEY (pueID_pk_fk, empDPI_fk),
    FOREIGN KEY (pueID_pk_fk) REFERENCES tblPUESTOS(pueID_pk),
    FOREIGN KEY (empDPI_fk) REFERENCES tblEMPLEADOS(empDPI_pk)
);

-- 4. Jornadas (requiere campeonato)
CREATE TABLE tblJORNADA (
    jorID_fk INT PRIMARY KEY AUTO_INCREMENT,
    camID_fk INT,
    canCANTIDAD_PARTIDOS INT,
    FOREIGN KEY (camID_fk) REFERENCES tblCAMPEONATOS(camID_pk)
);

-- 5. Partidos (requiere jornadas, empleados, estado, equipos, cancha)
CREATE TABLE tblPARTIDOS (
    parID_pk INT PRIMARY KEY AUTO_INCREMENT,
    jorID_fk INT,
    equiID_1 INT,
    equiID_2 INT,
    canID_fk INT,
    par_fecha_hora DATETIME,
    estID INT,
    empID_arbrito_fk BIGINT,
    parPuntaje_equipo1 INT,
    parPuntaje_equipo2 INT,
    parTIEMPO_EXTRA BOOLEAN,
    equID_ganador_fk INT,
    FOREIGN KEY (jorID_fk) REFERENCES tblJORNADA(jorID_fk),
    FOREIGN KEY (equiID_1) REFERENCES tblEQUIPOS(equID_pk),
    FOREIGN KEY (equiID_2) REFERENCES tblEQUIPOS(equID_pk),
    FOREIGN KEY (canID_fk) REFERENCES tblCANCHA(canID),
    FOREIGN KEY (estID) REFERENCES tblESTADO_PARTIDO(ID_ESTADO),
    FOREIGN KEY (empID_arbrito_fk) REFERENCES tblEMPLEADOS(empDPI_pk),
    FOREIGN KEY (equID_ganador_fk) REFERENCES tblEQUIPOS(equID_pk)
);

-- 6. Tablas que dependen de jugadores y partidos
CREATE TABLE tblFALTAS (
    falID_pk INT PRIMARY KEY AUTO_INCREMENT,
    falDESCRIPCION VARCHAR(255),
    falMinuto INT,
    falPartido INT,
    jugID_fk INT,
    FOREIGN KEY (falPartido) REFERENCES tblPARTIDOS(parID_pk),
    FOREIGN KEY (jugID_fk) REFERENCES tblJUGADORES(jugID_pk)
);

CREATE TABLE tblSANCIONES (
    sanID INT PRIMARY KEY AUTO_INCREMENT,
    sanDescripcion VARCHAR(255),
    sanMinuto INT,
    parID_fk INT,
    jugID_fk INT,
    FOREIGN KEY (parID_fk) REFERENCES tblPARTIDOS(parID_pk),
    FOREIGN KEY (jugID_fk) REFERENCES tblJUGADORES(jugID_pk)
);

CREATE TABLE tblANOTACION (
    anoID INT PRIMARY KEY AUTO_INCREMENT,
    anoMinuto INT,
    anoDESCRIPCION VARCHAR(255),
    parID_fk INT,
    jugID_pk INT,
    FOREIGN KEY (parID_fk) REFERENCES tblPARTIDOS(parID_pk),
    FOREIGN KEY (jugID_pk) REFERENCES tblJUGADORES(jugID_pk)
);

CREATE TABLE tblASISTENCIA_ANOTACION (
    asiID INT PRIMARY KEY AUTO_INCREMENT,
    anoID_fk INT,
    jugID INT,
    FOREIGN KEY (anoID_fk) REFERENCES tblANOTACION(anoID),
    FOREIGN KEY (jugID) REFERENCES tblJUGADORES(jugID_pk)
);

CREATE TABLE tbltiempo_Extra (
    tieID INT PRIMARY KEY AUTO_INCREMENT,
    tieDuracion INT,
    parID_fk INT,
    FOREIGN KEY (parID_fk) REFERENCES tblPARTIDOS(parID_pk)
);

CREATE TABLE tbl_entidad (
    pk_id_entidad INT PRIMARY KEY AUTO_INCREMENT,
    ent_nombre VARCHAR(50)
);

create table tbl_roles(
pk_id_roles INT primary key auto_increment,
priv_roles VARCHAR(50)
);

create table tbl_privilegios(
pk_id_privilegios INT primary key auto_increment,
priv_privilegios VARCHAR(300)
);

create table tbl_usuario(
pk_id_usuario INT primary key,
usuario VARCHAR(20),
user_email varchar(20),
user_contraseña varchar(50),
fk_id_privilegios INT,
fk_id_roles INT,
user_ultima_conexion datetime,
FOREIGN KEY (fk_id_privilegios) REFERENCES tbl_privilegios(pk_id_privilegios),
FOREIGN KEY (fk_id_roles) REFERENCES tbl_roles(pk_id_roles)
);
use polideportivo;
CREATE TABLE tbl_bitacora (
    pk_id_bitacora INT PRIMARY KEY AUTO_INCREMENT,
    fk_id_entidad INT,
    OPERACION CHAR,
    Fecha_Hora_Mod Datetime,
    fk_id_usuario INT,
    bit_ip_mod VARCHAR(15),
	FOREIGN KEY (fk_id_entidad) REFERENCES tbl_entidad(pk_id_entidad),
    FOREIGN KEY (fk_id_usuario) REFERENCES tbl_usuario(pk_id_usuario)
);