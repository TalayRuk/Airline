```Sql

-- ---
-- Globals
-- ---

-- SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
-- SET FOREIGN_KEY_CHECKS=0;

-- ---
-- Table 'cities'
--
-- ---

DROP TABLE IF EXISTS cities;

CREATE TABLE cities (
  id INTEGER IDENTITY(1,1),
  name VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (id)
);

-- ---
-- Table 'flights'
--
-- ---

DROP TABLE IF EXISTS flights;

CREATE TABLE flights (
  id INTEGER IDENTITY(1,1),
  flight_number VARCHAR(255) NULL DEFAULT NULL,
  depature_time DATETIME NULL DEFAULT NULL,
  arrival_time DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (id)
);

-- ---
-- Table 'flights_cities'
--
-- ---

DROP TABLE IF EXISTS flights_cities;

CREATE TABLE flights_cities (
  id INTEGER IDENTITY(1,1),
  flight INTEGER NULL DEFAULT NULL,
  destination_city INTEGER NULL DEFAULT NULL,
  departure_city INTEGER NULL DEFAULT NULL,
  status INTEGER NULL DEFAULT NULL,
  PRIMARY KEY (id)
);

-- ---
-- Table 'statuses'
--
-- ---

DROP TABLE IF EXISTS statuses;

CREATE TABLE statuses (
  id INTEGER IDENTITY(1,1),
  status VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (id)
);

-- ---
-- Foreign Keys
-- ---

ALTER TABLE flights_cities ADD FOREIGN KEY (flight) REFERENCES flights (id);
ALTER TABLE flights_cities ADD FOREIGN KEY (destination_city) REFERENCES cities (id);
ALTER TABLE flights_cities ADD FOREIGN KEY (departure_city) REFERENCES cities (id);
ALTER TABLE flights_cities ADD FOREIGN KEY (status) REFERENCES statuses (id);

-- ---
-- Table Properties
-- ---

-- ALTER TABLE cities ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE flights ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE flights_cities ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
-- ALTER TABLE statuses ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ---
-- Test Data
-- ---

-- INSERT INTO cities (id,name) VALUES
-- ('','');
-- INSERT INTO flights (id,flight_number,depature_time,arrival_time) VALUES
-- ('','','','');
-- INSERT INTO flights_cities (id,flight,destination_city,departure_city,status) VALUES
-- ('','','','','');
-- INSERT INTO statuses (id,status) VALUES
-- ('','');
```
