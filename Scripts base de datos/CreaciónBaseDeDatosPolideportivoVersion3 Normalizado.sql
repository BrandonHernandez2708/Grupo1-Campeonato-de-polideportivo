CREATE DATABASE Polideportivo;
USE Polideportivo;

-- 1. Tablas independientes
CREATE TABLE tbl_deporte (
    pk_deporte_id INT PRIMARY KEY,
    dep_nombre VARCHAR(100),
    dep_cant_jugadores_equipo INT,
    dep_cant_jugadores_campo INT,
    dep_cant_tiempos INT,
    dep_duracion_tiempo INT,
    dep_duracion_total INT
);

CREATE TABLE tbl_campeonato (
    pk_campeonato_id INT PRIMARY KEY,
    cam_nombre VARCHAR(100),
    cam_modalidad VARCHAR(50),
    cam_cant_equipos INT,
    cam_fecha_inicio DATE,
    cam_fecha_final DATE
);

CREATE TABLE tbl_entrenador (
    pk_entrenador_id INT PRIMARY KEY,
    ent_nombre VARCHAR(50),
    ent_apellido VARCHAR(50)
);

CREATE TABLE tbl_puesto (
    pk_puesto_id INT PRIMARY KEY,
    pue_nombre VARCHAR(50),
    pue_descripcion TEXT
);

CREATE TABLE tbl_estado_partido (
    pk_estado_partido_id INT PRIMARY KEY AUTO_INCREMENT,
    est_descripcion VARCHAR(100)
);

CREATE TABLE tbl_cancha (
    pk_cancha_id INT PRIMARY KEY AUTO_INCREMENT,
    can_capacidad INT,
    can_direccion VARCHAR(255)
);

CREATE TABLE tbl_entidad (
    pk_entidad_id INT PRIMARY KEY AUTO_INCREMENT,
    ent_nombre VARCHAR(50)
);

CREATE TABLE tbl_rol (
    pk_rol_id INT PRIMARY KEY AUTO_INCREMENT,
    rol_privilegio VARCHAR(50)
);

CREATE TABLE tbl_privilegio (
    pk_privilegio_id INT PRIMARY KEY AUTO_INCREMENT,
    priv_descripcion VARCHAR(300)
);

CREATE TABLE tbl_usuario (
    pk_usuario_id INT PRIMARY KEY,
    usu_nombre VARCHAR(20),
    usu_email VARCHAR(20),
    usu_contrasena VARCHAR(50),
    fk_privilegio_id INT,
    fk_rol_id INT,
    usu_ultima_conexion DATETIME,
    FOREIGN KEY (fk_privilegio_id) REFERENCES tbl_privilegio(pk_privilegio_id),
    FOREIGN KEY (fk_rol_id) REFERENCES tbl_rol(pk_rol_id)
);

-- 2. Tablas con claves foráneas simples
CREATE TABLE tbl_empleado (
    pk_empleado_id BIGINT PRIMARY KEY,
    emp_nombre VARCHAR(50),
    emp_apellido VARCHAR(50),
    fk_usuario_id INT,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id)
);

CREATE TABLE tbl_equipo (
    pk_equipo_id INT PRIMARY KEY,
    equ_nombre VARCHAR(100),
    fk_deporte_id INT,
    equ_cant_integrantes INT,
    fk_entrenador_id INT,
    equ_telefono VARCHAR(20),
    equ_correo VARCHAR(100),
    FOREIGN KEY (fk_deporte_id) REFERENCES tbl_deporte(pk_deporte_id),
    FOREIGN KEY (fk_entrenador_id) REFERENCES tbl_entrenador(pk_entrenador_id)
);

-- 3. Relaciones uno a muchos
CREATE TABLE tbl_telefono_empleado (
    pk_tel_empleado_id INT PRIMARY KEY,
    tel_numero VARCHAR(20),
    fk_empleado_id BIGINT,
    FOREIGN KEY (fk_empleado_id) REFERENCES tbl_empleado(pk_empleado_id)
);

CREATE TABLE tbl_correo_empleado (
    pk_correo_empleado_id INT PRIMARY KEY,
    correo VARCHAR(100),
    fk_empleado_id BIGINT,
    FOREIGN KEY (fk_empleado_id) REFERENCES tbl_empleado(pk_empleado_id)
);

CREATE TABLE tbl_telefono_entrenador (
    pk_tel_entrenador_id INT PRIMARY KEY,
    tel_numero VARCHAR(20),
    fk_entrenador_id INT,
    FOREIGN KEY (fk_entrenador_id) REFERENCES tbl_entrenador(pk_entrenador_id)
);

CREATE TABLE tbl_correo_entrenador (
    pk_cor_entrenador_id INT PRIMARY KEY,
    correo VARCHAR(100),
    fk_entrenador_id INT,
    FOREIGN KEY (fk_entrenador_id) REFERENCES tbl_entrenador(pk_entrenador_id)
);

CREATE TABLE tbl_jugador (
    pk_jugador_id INT PRIMARY KEY,
    jug_nombre VARCHAR(50),
    jug_apellido VARCHAR(50),
    jug_posicion VARCHAR(50),
    fk_equipo_id INT,
    FOREIGN KEY (fk_equipo_id) REFERENCES tbl_equipo(pk_equipo_id)
);

CREATE TABLE tbl_contratacion (
    fk_puesto_id INT,
    fk_empleado_id BIGINT,
    con_fecha DATE,
    con_tipo_operacion TINYINT,
    PRIMARY KEY (fk_puesto_id, fk_empleado_id),
    FOREIGN KEY (fk_puesto_id) REFERENCES tbl_puesto(pk_puesto_id),
    FOREIGN KEY (fk_empleado_id) REFERENCES tbl_empleado(pk_empleado_id)
);

-- 4. Jornadas
CREATE TABLE tbl_jornada (
    pk_jornada_id INT PRIMARY KEY AUTO_INCREMENT,
    fk_campeonato_id INT,
    jor_cant_partidos INT,
    FOREIGN KEY (fk_campeonato_id) REFERENCES tbl_campeonato(pk_campeonato_id)
);

-- 5. Partidos
CREATE TABLE tbl_partido (
    pk_partido_id INT PRIMARY KEY AUTO_INCREMENT,
    fk_jornada_id INT,
    fk_equipo1_id INT,
    fk_equipo2_id INT,
    fk_cancha_id INT,
    par_fecha_hora DATETIME,
    fk_estado_id INT,
    fk_empleado_arbitro_id BIGINT,
    par_puntaje_equipo1 INT,
    par_puntaje_equipo2 INT,
    par_tiempo_extra BOOLEAN,
    fk_equipo_ganador_id INT,
    FOREIGN KEY (fk_jornada_id) REFERENCES tbl_jornada(pk_jornada_id),
    FOREIGN KEY (fk_equipo1_id) REFERENCES tbl_equipo(pk_equipo_id),
    FOREIGN KEY (fk_equipo2_id) REFERENCES tbl_equipo(pk_equipo_id),
    FOREIGN KEY (fk_cancha_id) REFERENCES tbl_cancha(pk_cancha_id),
    FOREIGN KEY (fk_estado_id) REFERENCES tbl_estado_partido(pk_estado_partido_id),
    FOREIGN KEY (fk_empleado_arbitro_id) REFERENCES tbl_empleado(pk_empleado_id),
    FOREIGN KEY (fk_equipo_ganador_id) REFERENCES tbl_equipo(pk_equipo_id)
);

-- 6. Eventos del partido
CREATE TABLE tbl_falta (
    pk_falta_id INT PRIMARY KEY AUTO_INCREMENT,
    fal_descripcion VARCHAR(255),
    fal_minuto INT,
    fk_partido_id INT,
    fk_jugador_id INT,
    FOREIGN KEY (fk_partido_id) REFERENCES tbl_partido(pk_partido_id),
    FOREIGN KEY (fk_jugador_id) REFERENCES tbl_jugador(pk_jugador_id)
);

CREATE TABLE tbl_sancion (
    pk_sancion_id INT PRIMARY KEY AUTO_INCREMENT,
    san_descripcion VARCHAR(255),
    san_minuto INT,
    fk_partido_id INT,
    fk_jugador_id INT,
    FOREIGN KEY (fk_partido_id) REFERENCES tbl_partido(pk_partido_id),
    FOREIGN KEY (fk_jugador_id) REFERENCES tbl_jugador(pk_jugador_id)
);

CREATE TABLE tbl_anotacion (
    pk_anotacion_id INT PRIMARY KEY AUTO_INCREMENT,
    ano_minuto INT,
    ano_descripcion VARCHAR(255),
    fk_partido_id INT,
    fk_jugador_id INT,
    FOREIGN KEY (fk_partido_id) REFERENCES tbl_partido(pk_partido_id),
    FOREIGN KEY (fk_jugador_id) REFERENCES tbl_jugador(pk_jugador_id)
);

CREATE TABLE tbl_asistencia (
    pk_asistencia_id INT PRIMARY KEY AUTO_INCREMENT,
    fk_anotacion_id INT,
    fk_jugador_id INT,
    FOREIGN KEY (fk_anotacion_id) REFERENCES tbl_anotacion(pk_anotacion_id),
    FOREIGN KEY (fk_jugador_id) REFERENCES tbl_jugador(pk_jugador_id)
);

CREATE TABLE tbl_tiempo_extra (
    pk_tiempo_extra_id INT PRIMARY KEY AUTO_INCREMENT,
    tie_duracion INT,
    fk_partido_id INT,
    FOREIGN KEY (fk_partido_id) REFERENCES tbl_partido(pk_partido_id)
);

-- 7. Bitácora
CREATE TABLE tbl_bitacora (
    pk_bitacora_id INT PRIMARY KEY AUTO_INCREMENT,
    fk_entidad_id INT,
    bit_operacion CHAR,
    bit_fecha_hora DATETIME,
    fk_usuario_id INT,
    bit_ip VARCHAR(15),
    FOREIGN KEY (fk_entidad_id) REFERENCES tbl_entidad(pk_entidad_id),
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id)
);
