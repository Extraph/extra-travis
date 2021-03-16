CREATE USER adminijoin_user WITH PASSWORD 'adminijoin_password' CREATEDB;
CREATE DATABASE adminijoin_database
    WITH 
    OWNER = adminijoin_user
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

ALTER DATABASE adminijoin_database SET "TimeZone" TO 'Asia/Bangkok';


CREATE USER userijoin_user WITH PASSWORD 'userijoin_password' CREATEDB;
CREATE DATABASE userijoin_database
    WITH 
    OWNER = userijoin_user
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

ALTER DATABASE userijoin_database SET "TimeZone" TO 'Asia/Bangkok';