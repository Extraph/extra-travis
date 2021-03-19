-- dotnet tool install --global dotnet-ef

-- dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=3306;Database=adminijoin_database;Username=root;Password=password" MySql.Data.EntityFrameworkCore -o EfAdminModels -f;
-- dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=3306;Database=userijoin_database;Username=root;Password=password" MySql.Data.EntityFrameworkCore -o EfUserModels -f;

DROP DATABASE `adminijoin_database`;
DROP DATABASE `userijoin_database`;
CREATE DATABASE IF NOT EXISTS `adminijoin_database` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;
CREATE DATABASE IF NOT EXISTS `userijoin_database` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci;

SET GLOBAL time_zone = '+7:00';


-- DROP TABLE adminijoin_database.TBM_SESSION_USER;
-- DROP TABLE adminijoin_database.TBM_SESSION_USER_HIS;
-- DROP TABLE adminijoin_database.TBM_REGISTRATION_STATUS;
-- DROP TABLE adminijoin_database.TBM_SEGMENT;
-- DROP TABLE adminijoin_database.TBM_SESSION;
-- DROP TABLE adminijoin_database.TBM_COURSE_TYPE;
-- DROP TABLE adminijoin_database.TBM_COURSE;
-- DROP TABLE adminijoin_database.TB_USER_COMPANY;
-- DROP TABLE adminijoin_database.TBM_COMPANY;
-- DROP TABLE adminijoin_database.TBM_USER;
-- DROP TABLE adminijoin_database.TBM_ROLE;
-- DROP TABLE adminijoin_database.TB_KLC_DATA_MASTER_HIS;
-- DROP TABLE adminijoin_database.TB_KLC_DATA_MASTER;
-- DROP TABLE adminijoin_database.TBM_KLC_FILE_IMPORT;



CREATE TABLE adminijoin_database.TBM_COMPANY
(
  company_id int NOT NULL AUTO_INCREMENT,
  company_code varchar(100) NOT NULL,
  create_by varchar(45) DEFAULT NULL,
  create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  update_by varchar(45) DEFAULT NULL,
  update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (company_id),
  UNIQUE KEY company_code_UNIQUE (company_code)
);

CREATE TABLE adminijoin_database.TBM_ROLE
(
  role_id int NOT NULL AUTO_INCREMENT,
  role_name varchar(100) NOT NULL,
  create_by varchar(45) DEFAULT NULL,
  create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  update_by varchar(45) DEFAULT NULL,
  update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (role_id)
);

  INSERT INTO adminijoin_database.TBM_ROLE(role_name) VALUES ('Super Admin');
  INSERT INTO adminijoin_database.TBM_ROLE(role_name) VALUES ('Admin');
  INSERT INTO adminijoin_database.TBM_ROLE(role_name) VALUES ('Report Admin');
  INSERT INTO adminijoin_database.TBM_ROLE(role_name) VALUES ('Instructor'); 

CREATE TABLE adminijoin_database.TBM_USER
(
  user_id varchar(100) NOT NULL,
  user_password varchar(100) DEFAULT NULL,
  user_name varchar(100) DEFAULT NULL,
  user_organization varchar(100) DEFAULT NULL,
  user_company varchar(100) DEFAULT NULL,
  role_id int NOT NULL,
  create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id)
);

  INSERT INTO adminijoin_database.TBM_USER(user_id, role_id) VALUES ('7E020390', 1);

CREATE TABLE adminijoin_database.TB_USER_COMPANY
(
  id int NOT NULL AUTO_INCREMENT,
  user_id varchar(100) NOT NULL,
  company_id int NOT NULL,
  is_default CHAR(1) NOT NULL DEFAULT '0',
  create_by varchar(45) DEFAULT NULL,
  create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
);

CREATE TABLE adminijoin_database.TBM_KLC_FILE_IMPORT
(
  id int NOT NULL AUTO_INCREMENT,
  file_name varchar(200) NOT NULL,
  status varchar(200) NOT NULL,
  guid_name varchar(200) NOT NULL,
  import_by varchar(45) NOT NULL,
  import_type varchar(45) DEFAULT NULL,
  import_totalrecords varchar(45) DEFAULT NULL,
  import_message varchar(45) DEFAULT NULL,
  create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id)
);

--------------------------------------------
--------------------------------------------
--------------------------------------------

CREATE TABLE adminijoin_database.TB_KLC_DATA_MASTER
(
    id int NOT NULL AUTO_INCREMENT,
    file_id int NOT NULL,
    course_type varchar(500) DEFAULT NULL,
    course_id varchar(500) NOT NULL,
    course_name varchar(500) NOT NULL,
    course_name_th varchar(500) DEFAULT NULL,
    session_id varchar(500) NOT NULL,
    session_name varchar(500) NOT NULL,
    segment_no varchar(500) NOT NULL,
    segment_name varchar(500) DEFAULT NULL,
    start_date varchar(500) NOT NULL,
    end_date varchar(500) NOT NULL,
    start_time varchar(500) NOT NULL,
    end_time varchar(500) NOT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    course_owner_email varchar(500) NOT NULL,
    course_owner_contact_no varchar(500) DEFAULT NULL,
    venue varchar(500) DEFAULT NULL,
    instructor varchar(500) DEFAULT NULL,
    course_credit_hours varchar(500) DEFAULT NULL,
    passing_criteria_exception varchar(500) DEFAULT NULL,
    user_company varchar(500) DEFAULT NULL,
    user_id varchar(500) NOT NULL,
    registration_status varchar(500) NOT NULL,
    invalid_message varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    CONSTRAINT fk_tbm_klc_file_import_id FOREIGN KEY (file_id) REFERENCES adminijoin_database.TBM_KLC_FILE_IMPORT (id)
);

CREATE TABLE adminijoin_database.TB_KLC_DATA_MASTER_HIS
(
    id int NOT NULL,
    file_id int NOT NULL,
    course_type varchar(500) DEFAULT NULL,
    course_id varchar(500) NOT NULL,
    course_name varchar(500) NOT NULL,
    course_name_th varchar(500) DEFAULT NULL,
    session_id varchar(500) NOT NULL,
    session_name varchar(500) NOT NULL,
    segment_no varchar(500) NOT NULL,
    segment_name varchar(500) DEFAULT NULL,
    start_date varchar(500) NOT NULL,
    end_date varchar(500) NOT NULL,
    start_time varchar(500) NOT NULL,
    end_time varchar(500) NOT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    course_owner_email varchar(500) NOT NULL,
    course_owner_contact_no varchar(500) DEFAULT NULL,
    venue varchar(500) DEFAULT NULL,
    instructor varchar(500) DEFAULT NULL,
    course_credit_hours varchar(500) DEFAULT NULL,
    passing_criteria_exception varchar(500) DEFAULT NULL,
    user_company varchar(500) DEFAULT NULL,
    user_id varchar(500) NOT NULL,
    registration_status varchar(500) NOT NULL,
    invalid_message varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    CONSTRAINT fk_tbm_klc_file_import_id_his FOREIGN KEY (file_id) REFERENCES adminijoin_database.TBM_KLC_FILE_IMPORT (id)
);

CREATE TABLE adminijoin_database.TBM_COURSE_TYPE
(
    id int NOT NULL AUTO_INCREMENT,
    course_type varchar(500) NOT NULL,
    completion_status varchar(500) NOT NULL,
    description varchar(500) NOT NULL,
    create_by varchar(500) DEFAULT NULL,
    create_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_by varchar(500) DEFAULT NULL,
    update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id)
);
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('In-house', 'In-house_Completed', 'Completed');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('In-house', 'In-house_Incompleted', 'Incompleted');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('In-house', 'In-house_No Show', 'No Show');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('In-house', 'In-house_Cancelled', 'Cancelled');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Completed', 'Completed');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Incompleted', 'Incompleted');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_No Show', 'No Show');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Cancelled', 'Cancelled');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('On-the-Job Training', 'OJT_Completed', 'Completed');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('On-the-Job Training', 'OJT_Inompleted', 'Incompleted');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Completed', 'Completed');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Incompleted', 'Incompleted');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_No Show', 'No Show');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Cancelled', 'Cancelled');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Completed', 'Completed');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Incomplete', 'Incompleted');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_No Show', 'No Show');
INSERT INTO adminijoin_database.TBM_COURSE_TYPE(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Cancelled', 'Cancelled');

CREATE TABLE adminijoin_database.TBM_COURSE
(
    course_id varchar(500) NOT NULL,
    course_name varchar(500) NOT NULL,
    course_name_th varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (course_id)
);

CREATE TABLE adminijoin_database.TBM_REGISTRATION_STATUS
(
    id int NOT NULL AUTO_INCREMENT,
    registration_status varchar(500) NOT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    UNIQUE KEY registration_status_UNIQUE (registration_status)
);
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Enrolled');
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Waitlist');
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Cancelled'); 
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Check-In');
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Check-Out');
INSERT INTO adminijoin_database.TBM_REGISTRATION_STATUS(registration_status) VALUES ('Deleted');

CREATE TABLE adminijoin_database.TBM_SESSION
(
    file_id int NOT NULL,
    course_type varchar(500) NOT NULL,
    company_id int NOT NULL,
    company_code varchar(500) NOT NULL,
    course_id varchar(500) NOT NULL,
    course_name varchar(500) NOT NULL,
    course_name_th varchar(500) DEFAULT NULL,
    session_id varchar(500) NOT NULL,
    session_name varchar(500) DEFAULT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    course_owner_email varchar(500) NOT NULL,
    course_owner_contact_no varchar(500) DEFAULT NULL,
    venue varchar(500) DEFAULT NULL,
    instructor varchar(500) DEFAULT NULL,
    course_credit_hours_init varchar(500) DEFAULT NULL,
    passing_criteria_exception_init varchar(500) DEFAULT NULL,
    course_credit_hours varchar(500) DEFAULT NULL,
    passing_criteria_exception varchar(500) DEFAULT NULL,
    is_cancel CHAR(1) NOT NULL DEFAULT '0',
    cover_photo_name varchar(500) DEFAULT NULL,
    cover_photo_url varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_by varchar(500) DEFAULT NULL,
    update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id),
    KEY idx_course_id (course_id),
    KEY idx_session_id (session_id),
    CONSTRAINT fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id FOREIGN KEY (file_id) REFERENCES adminijoin_database.TBM_KLC_FILE_IMPORT (id)
);

CREATE TABLE adminijoin_database.TBM_SEGMENT
(
    session_id varchar(500) NOT NULL,
    segment_no varchar(500) NOT NULL,
    segment_name varchar(500) DEFAULT NULL,
    start_date varchar(500) NOT NULL,
    end_date varchar(500) NOT NULL,
    start_time varchar(500) NOT NULL,
    end_time varchar(500) NOT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    venue varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id, start_date_time, end_date_time),
    CONSTRAINT fk_TBM_SESSION_TBM_SEGMENT_session_id FOREIGN KEY (session_id) REFERENCES adminijoin_database.TBM_SESSION (session_id) 
);

CREATE TABLE adminijoin_database.TBM_SESSION_USER
(
    session_id varchar(500) NOT NULL,
    user_id varchar(500) NOT NULL,
    registration_status varchar(500) NOT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_by varchar(500) DEFAULT NULL,
    update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id, user_id),
    KEY idx_session_id (user_id),
    KEY idx_session_id_user_id (session_id,user_id),
    CONSTRAINT fk_TBM_SESSION_TBM_SESSION_USER_session_id FOREIGN KEY (session_id) REFERENCES adminijoin_database.TBM_SESSION (session_id)
);

CREATE TABLE adminijoin_database.TBM_SESSION_USER_HIS
(
    session_id varchar(500) NOT NULL,
    user_id varchar(500) NOT NULL,
    registration_status varchar(500) NOT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_by varchar(500) DEFAULT NULL,
    update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id, user_id),
    KEY idx_session_id (user_id),
    KEY idx_session_id_user_id (session_id,user_id),
    CONSTRAINT fk_TBM_SESSION_TBM_SESSION_USER_HIS_session_id FOREIGN KEY (session_id) REFERENCES adminijoin_database.TBM_SESSION (session_id)
);




-- CREATE OR REPLACE VIEW adminijoin_database.v_Segment_Gen_Qr"
--  AS
--  SELECT ss.course_id,
--     ss.course_name,
--     ss.course_name_th,
--     ss.session_id,
--     ss.session_name,
--     sg.venue,
--     sg.segment_name,
--     to_timestamp(((to_char(sg.intervalday, 'YYYY-MM-DD'::text) || ' '::text) || to_char(sg.start_date_time, 'HH24:MI:SS'::text)), 'YYYY-MM-DD HH24:MI:SS'::text) AS start_date_time,
--     to_timestamp(((to_char(sg.intervalday, 'YYYY-MM-DD'::text) || ' '::text) || to_char(sg.end_date_time, 'HH24:MI:SS'::text)), 'YYYY-MM-DD HH24:MI:SS'::text) AS end_date_time
--    FROM (( SELECT "TBM_SEGMENT".session_id,
--             "TBM_SEGMENT".start_date,
--             "TBM_SEGMENT".end_date,
--             min("TBM_SEGMENT".start_date_time) AS start_date_time,
--             max("TBM_SEGMENT".end_date_time) AS end_date_time,
--             max("TBM_SEGMENT".venue) AS venue,
--             max("TBM_SEGMENT".segment_name) AS segment_name,
--             generate_series(((min("TBM_SEGMENT".start_date_time))::date)::timestamp with time zone, ((max("TBM_SEGMENT".end_date_time))::date)::timestamp with time zone, '1 day'::interval) AS intervalday
--            FROM "TBM_SEGMENT"
--           GROUP BY "TBM_SEGMENT".session_id, "TBM_SEGMENT".start_date, "TBM_SEGMENT".end_date
--           ORDER BY "TBM_SEGMENT".session_id, (min("TBM_SEGMENT".start_date_time))) sg
--      JOIN "TBM_SESSION" ss ON ((sg.session_id = ss.session_id)));

-- ALTER TABLE adminijoin_database.v_Segment_Gen_Qr"
--     OWNER TO adminijoin_user;




----------------------------------------------------------------
----------------------------------------------------------------
----------------------------------------------------------------





CREATE TABLE userijoin_database.TBM_USER_SESSION
(
    course_id varchar(500) NOT NULL,
    course_name varchar(500) NOT NULL,
    course_name_th varchar(500) DEFAULT NULL,
    session_id varchar(500) NOT NULL,
    session_name varchar(500) DEFAULT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    course_owner_email varchar(500) NOT NULL,
    course_owner_contact_no varchar(500) DEFAULT NULL,
    venue varchar(500) DEFAULT NULL,
    instructor varchar(500) DEFAULT NULL,
    course_credit_hours_init varchar(500) DEFAULT NULL,
    passing_criteria_exception_init varchar(500) DEFAULT NULL,
    course_credit_hours varchar(500) DEFAULT NULL,
    passing_criteria_exception varchar(500) DEFAULT NULL,
    is_cancel CHAR(1) NOT NULL DEFAULT '0',
    cover_photo_name varchar(500) DEFAULT NULL,
    cover_photo_url varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_by varchar(500) DEFAULT NULL,
    update_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id),
    KEY idx_course_id (course_id),
    KEY idx_session_id (session_id)
);

CREATE TABLE userijoin_database.TBM_USER_SEGMENT
(
    session_id varchar(500) NOT NULL,
    segment_name varchar(500) DEFAULT NULL,
    start_date varchar(500) NOT NULL,
    end_date varchar(500) NOT NULL,
    start_time varchar(500) NOT NULL,
    end_time varchar(500) NOT NULL,
    start_date_time datetime NOT NULL,
    end_date_time datetime NOT NULL,
    venue varchar(500) DEFAULT NULL,
    createdatetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (session_id, start_date_time, end_date_time),
    CONSTRAINT fk_TBM_USER_SESSION_TBM_USER_SEGMENT_session_id FOREIGN KEY (session_id) REFERENCES userijoin_database.TBM_USER_SESSION (session_id) 
);

CREATE TABLE userijoin_database.TBM_USER_SESSION_USER
(
    session_id varchar(500) NOT NULL,
    user_id varchar(500) NOT NULL,
    PRIMARY KEY (session_id, user_id),
    KEY idx_session_id (user_id),
    KEY idx_session_id_user_id (session_id,user_id),
    CONSTRAINT fk_TBM_SESSION_TBM_SESSION_USER_session_id FOREIGN KEY (session_id) REFERENCES userijoin_database.TBM_USER_SESSION (session_id)
);

CREATE TABLE userijoin_database.TB_USER_REGISTRATION
(
    id int NOT NULL AUTO_INCREMENT,
    session_id varchar(500) NOT NULL,
    user_id varchar(500) NOT NULL,
    start_date_qr varchar(500) DEFAULT NULL,
    is_check_in CHAR(1) NOT NULL DEFAULT '0',
    is_check_out CHAR(1) NOT NULL DEFAULT '0',
    check_in_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    check_in_date varchar(500) DEFAULT NULL,
    check_in_time int NOT NULL,
    check_out_datetime datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    check_out_date varchar(500) DEFAULT NULL,
    check_out_time int NOT NULL,
    check_in_by varchar(500) DEFAULT NULL,
    check_out_by varchar(500) DEFAULT NULL,
    PRIMARY KEY (id),
    KEY idx_check_in_datetime (check_in_datetime),
    KEY idx_check_in_date (check_in_date),
    KEY idx_session_id (session_id),
    KEY idx_user_id (user_id),
    KEY idx_com (check_in_date,check_in_datetime)
)