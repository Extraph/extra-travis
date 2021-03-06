
--dotnet tool install --global dotnet-ef

--dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=5432;Database=adminijoin_database;Username=adminijoin_user;Password=adminijoin_password" Npgsql.EntityFrameworkCore.PostgreSQL -o EfAdminModels -f

--ALTER DATABASE adminijoin_database SET timezone TO 'Asia/Bangkok';

--[Course Master] 
--course_type (*required)
--course_id (*required)
--course_name (*required)	
--course_name_th	
--session_id (*required)	
--session_name (*required)	
--segment_no (*required)	
--segment_name	
--start_date (*required)	
--end_date (*required)	
--start_time (*required)	
--end_time (*required)	
--course_owner_email (*required)	
--course_owner_contactNo
--venue
--instructor
--course_credit_hours
--passing_criteria_exception
--user_company
--user_id (*required)	
--registration_status (*required)


------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

ALTER DATABASE adminijoin_database SET "TimeZone" TO 'Asia/Bangkok';

CREATE SEQUENCE public."TBM_KLC_FILE_IMPORT_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TBM_KLC_FILE_IMPORT_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TB_KLC_DATA_MASTER_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;

ALTER SEQUENCE public."TB_KLC_DATA_MASTER_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TBM_COURSE_TYPE_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TBM_COURSE_TYPE_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TBM_REGISTRATION_STATUS_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TBM_REGISTRATION_STATUS_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TBM_SEGMENT_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 9223372036854775807
    CACHE 1;

ALTER SEQUENCE public."TBM_SEGMENT_id_seq"
    OWNER TO adminijoin_user;

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

CREATE SEQUENCE public."TBM_COMPANY_company_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TBM_COMPANY_company_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TBM_ROLE_role_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TBM_ROLE_role_id_seq"
    OWNER TO adminijoin_user;

CREATE SEQUENCE public."TB_USER_COMPANY_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TB_USER_COMPANY_id_seq"
    OWNER TO adminijoin_user;

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

CREATE TABLE public."TBM_COMPANY"
(
    company_id integer NOT NULL DEFAULT nextval('"TBM_COMPANY_company_id_seq"'::regclass),
    company_code text COLLATE pg_catalog."default" NOT NULL,
    create_by text COLLATE pg_catalog."default",
    create_datetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_COMPANY_pkey" PRIMARY KEY (company_id),
    CONSTRAINT uni_company_code UNIQUE (company_code)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_COMPANY"
    OWNER to adminijoin_user;


CREATE TABLE public."TBM_ROLE"
(
    role_id integer NOT NULL DEFAULT nextval('"TBM_ROLE_role_id_seq"'::regclass),
    role_name text COLLATE pg_catalog."default" NOT NULL,
    create_by text COLLATE pg_catalog."default",
    create_datetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_ROLE_pkey" PRIMARY KEY (role_id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_ROLE"
    OWNER to adminijoin_user;

    INSERT INTO public."TBM_ROLE"(role_name) VALUES ('Super Admin');
    INSERT INTO public."TBM_ROLE"(role_name) VALUES ('Admin');
    INSERT INTO public."TBM_ROLE"(role_name) VALUES ('Report Admin');
    INSERT INTO public."TBM_ROLE"(role_name) VALUES ('Instructor'); 
    


CREATE TABLE public."TBM_USER"
(
    user_id text COLLATE pg_catalog."default" NOT NULL,
    user_password text COLLATE pg_catalog."default",
    user_name text COLLATE pg_catalog."default",
    user_organization text COLLATE pg_catalog."default",
    user_company text COLLATE pg_catalog."default",
    role_id integer NOT NULL,
    create_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_USER_pkey" PRIMARY KEY (user_id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_USER"
    OWNER to adminijoin_user;

    INSERT INTO public."TBM_USER"(user_id, role_id) VALUES ('7E020390', 1);


CREATE TABLE public."TB_USER_COMPANY"
(
    id integer NOT NULL DEFAULT nextval('"TB_USER_COMPANY_id_seq"'::regclass),
	user_id text COLLATE pg_catalog."default" NOT NULL,
    company_id integer NOT NULL,
    is_default character(1) COLLATE pg_catalog."default" NOT NULL DEFAULT '0'::bpchar,
    create_by text COLLATE pg_catalog."default",
    create_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TB_USER_COMPANY_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public."TB_USER_COMPANY"
    OWNER to adminijoin_user;


------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------


-- Table: public.TBM_KLC_FILE_IMPORT

-- DROP TABLE public."TBM_KLC_FILE_IMPORT";

CREATE TABLE public."TBM_KLC_FILE_IMPORT"
(
    id integer NOT NULL DEFAULT nextval('"TBM_KLC_FILE_IMPORT_id_seq"'::regclass),
    file_name text COLLATE pg_catalog."default" NOT NULL,
    status text COLLATE pg_catalog."default" NOT NULL,
    guid_name text COLLATE pg_catalog."default" NOT NULL,
    import_by text COLLATE pg_catalog."default" NOT NULL,
    import_type text COLLATE pg_catalog."default",
    import_totalrecords text COLLATE pg_catalog."default",
    import_message text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_KLC_FILE_IMPORT_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_KLC_FILE_IMPORT"
    OWNER to adminijoin_user;

COMMENT ON COLUMN public."TBM_KLC_FILE_IMPORT".status
    IS 'upload success, import success, import failed';



-- Table: public.TB_KLC_DATA_MASTER

-- DROP TABLE public."TB_KLC_DATA_MASTER";

CREATE TABLE public."TB_KLC_DATA_MASTER"
(
    id bigint NOT NULL DEFAULT nextval('"TB_KLC_DATA_MASTER_id_seq"'::regclass),
    file_id integer NOT NULL,
    course_type text COLLATE pg_catalog."default",
    course_id text COLLATE pg_catalog."default" NOT NULL,
    course_name text COLLATE pg_catalog."default" NOT NULL,
    course_name_th text COLLATE pg_catalog."default",
    session_id text COLLATE pg_catalog."default" NOT NULL,
    session_name text COLLATE pg_catalog."default" NOT NULL,
    segment_no text COLLATE pg_catalog."default" NOT NULL,
    segment_name text COLLATE pg_catalog."default",
    start_date text COLLATE pg_catalog."default" NOT NULL,
    end_date text COLLATE pg_catalog."default" NOT NULL,
    start_time text COLLATE pg_catalog."default" NOT NULL,
    end_time text COLLATE pg_catalog."default" NOT NULL,
    start_date_time timestamp without time zone NOT NULL,
    end_date_time timestamp without time zone NOT NULL,
    course_owner_email text COLLATE pg_catalog."default" NOT NULL,
    course_owner_contact_no text COLLATE pg_catalog."default",
    venue text COLLATE pg_catalog."default",
    instructor text COLLATE pg_catalog."default",
    course_credit_hours text COLLATE pg_catalog."default",
    passing_criteria_exception text COLLATE pg_catalog."default",
    user_company text COLLATE pg_catalog."default",
    user_id text COLLATE pg_catalog."default" NOT NULL,
    registration_status text COLLATE pg_catalog."default" NOT NULL,
    invalid_message text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TB_KLC_DATA_MASTER_pkey" PRIMARY KEY (id),
    CONSTRAINT fk_tbm_klc_file_import_id FOREIGN KEY (file_id)
        REFERENCES public."TBM_KLC_FILE_IMPORT" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TB_KLC_DATA_MASTER"
    OWNER to adminijoin_user;
-- Index: fki_fk_tbm_klc_file_import_id

-- DROP INDEX public.fki_fk_tbm_klc_file_import_id;

CREATE INDEX fki_fk_tbm_klc_file_import_id
    ON public."TB_KLC_DATA_MASTER" USING btree
    (file_id ASC NULLS LAST)
    TABLESPACE pg_default;


-- Table: public.TB_KLC_DATA_MASTER_HIS

-- DROP TABLE public."TB_KLC_DATA_MASTER_HIS";

CREATE TABLE public."TB_KLC_DATA_MASTER_HIS"
(
    id bigint NOT NULL,
    file_id integer NOT NULL,
    course_type text COLLATE pg_catalog."default",
    course_id text COLLATE pg_catalog."default" NOT NULL,
    course_name text COLLATE pg_catalog."default" NOT NULL,
    course_name_th text COLLATE pg_catalog."default",
    session_id text COLLATE pg_catalog."default" NOT NULL,
    session_name text COLLATE pg_catalog."default" NOT NULL,
    segment_no text COLLATE pg_catalog."default" NOT NULL,
    segment_name text COLLATE pg_catalog."default",
    start_date text COLLATE pg_catalog."default" NOT NULL,
    end_date text COLLATE pg_catalog."default" NOT NULL,
    start_time text COLLATE pg_catalog."default" NOT NULL,
    end_time text COLLATE pg_catalog."default" NOT NULL,
    start_date_time timestamp without time zone NOT NULL,
    end_date_time timestamp without time zone NOT NULL,
    course_owner_email text COLLATE pg_catalog."default" NOT NULL,
    course_owner_contact_no text COLLATE pg_catalog."default",
    venue text COLLATE pg_catalog."default",
    instructor text COLLATE pg_catalog."default",
    course_credit_hours text COLLATE pg_catalog."default",
    passing_criteria_exception text COLLATE pg_catalog."default",
    user_company text COLLATE pg_catalog."default",
    user_id text COLLATE pg_catalog."default" NOT NULL,
    registration_status text COLLATE pg_catalog."default" NOT NULL,
    invalid_message text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TB_KLC_DATA_MASTER_HIS_pkey" PRIMARY KEY (id),
    CONSTRAINT fk_tbm_klc_file_import_id_his FOREIGN KEY (file_id)
        REFERENCES public."TBM_KLC_FILE_IMPORT" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TB_KLC_DATA_MASTER_HIS"
    OWNER to adminijoin_user;
-- Index: fki_fk_tbm_klc_file_import_id_his

-- DROP INDEX public.fki_fk_tbm_klc_file_import_id_his;

CREATE INDEX fki_fk_tbm_klc_file_import_id_his
    ON public."TB_KLC_DATA_MASTER_HIS" USING btree
    (file_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- Table: public.TBM_COURSE_TYPE

-- DROP TABLE public."TBM_COURSE_TYPE";

CREATE TABLE public."TBM_COURSE_TYPE"
(
    id integer NOT NULL DEFAULT nextval('"TBM_COURSE_TYPE_id_seq"'::regclass),
    course_type text COLLATE pg_catalog."default" NOT NULL,
    completion_status text COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default" NOT NULL,
    create_by text COLLATE pg_catalog."default",
    create_datetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_COURSE_TYPE_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_COURSE_TYPE"
    OWNER to adminijoin_user;

INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('In-house', 'In-house_Completed', 'Completed');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('In-house', 'In-house_Incompleted', 'Incompleted');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('In-house', 'In-house_No Show', 'No Show');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('In-house', 'In-house_Cancelled', 'Cancelled');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Completed', 'Completed');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Incompleted', 'Incompleted');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_No Show', 'No Show');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Dev. Activity - In-house', 'Activity-Inhouse_Cancelled', 'Cancelled');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('On-the-Job Training', 'OJT_Completed', 'Completed');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('On-the-Job Training', 'OJT_Inompleted', 'Incompleted');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Completed', 'Completed');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Incompleted', 'Incompleted');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_No Show', 'No Show');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Webinar/ VR/ AR', 'Webinar/ VR/ AR_Cancelled', 'Cancelled');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Completed', 'Completed');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Incomplete', 'Incompleted');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_No Show', 'No Show');
INSERT INTO public."TBM_COURSE_TYPE"(course_type, completion_status, description) VALUES ('Coaching/ Mentoring', 'Coaching/ Mentoring_Cancelled', 'Cancelled');



-- Table: public.TBM_COURSE

-- DROP TABLE public."TBM_COURSE";

CREATE TABLE public."TBM_COURSE"
(
    course_id text COLLATE pg_catalog."default" NOT NULL,
    course_name text COLLATE pg_catalog."default" NOT NULL,
    course_name_th text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_COURSE_pkey" PRIMARY KEY (course_id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_COURSE"
    OWNER to adminijoin_user;    



-- Table: public.TBM_REGISTRATION_STATUS

-- DROP TABLE public."TBM_REGISTRATION_STATUS";

CREATE TABLE public."TBM_REGISTRATION_STATUS"
(
    id integer NOT NULL DEFAULT nextval('"TBM_REGISTRATION_STATUS_id_seq"'::regclass),
    registration_status text COLLATE pg_catalog."default" NOT NULL,
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_REGISTRATION_STATUS_pkey" PRIMARY KEY (id),
    CONSTRAINT "uni_TBM_REGISTRATION_STATUS" UNIQUE (registration_status)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_REGISTRATION_STATUS"
    OWNER to adminijoin_user;

    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Enrolled');
    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Waitlist');
    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Cancelled'); 
    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Check-In');
    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Check-Out');
    INSERT INTO public."TBM_REGISTRATION_STATUS"(registration_status) VALUES ('Deleted');





-- Table: public.TBM_SESSION

-- DROP TABLE public."TBM_SESSION";

CREATE TABLE public."TBM_SESSION"
(
    file_id integer NOT NULL,
    course_type text COLLATE pg_catalog."default" NOT NULL,
    company_id integer NOT NULL,
    company_code text COLLATE pg_catalog."default" NOT NULL,
    course_id text COLLATE pg_catalog."default" NOT NULL,
    course_name text COLLATE pg_catalog."default" NOT NULL,
    course_name_th text COLLATE pg_catalog."default",
    session_id text COLLATE pg_catalog."default" NOT NULL,
    session_name text COLLATE pg_catalog."default",
    start_date_time timestamp without time zone NOT NULL,
    end_date_time timestamp without time zone NOT NULL,
    course_owner_email text COLLATE pg_catalog."default" NOT NULL,
    course_owner_contact_no text COLLATE pg_catalog."default",
    venue text COLLATE pg_catalog."default",
    instructor text COLLATE pg_catalog."default",
    course_credit_hours_init text COLLATE pg_catalog."default",
    passing_criteria_exception_init text COLLATE pg_catalog."default",
    course_credit_hours text COLLATE pg_catalog."default",
    passing_criteria_exception text COLLATE pg_catalog."default",
    is_cancel character(1) COLLATE pg_catalog."default" NOT NULL DEFAULT '0'::bpchar,
    cover_photo_name text COLLATE pg_catalog."default",
    cover_photo_url text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_SESSION_pkey" PRIMARY KEY (session_id),
    CONSTRAINT "fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id" FOREIGN KEY (file_id)
        REFERENCES public."TBM_KLC_FILE_IMPORT" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_SESSION"
    OWNER to adminijoin_user;
-- Index: fki_fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id

-- DROP INDEX public."fki_fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id";

CREATE INDEX "fki_fk_SESSION_TO_TBM_KLC_FILE_IMPORT_id"
    ON public."TBM_SESSION" USING btree
    (file_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_TBM_SESSION_course_id

-- DROP INDEX public."idx_TBM_SESSION_course_id";

CREATE INDEX "idx_TBM_SESSION_course_id"
    ON public."TBM_SESSION" USING btree
    (course_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_TBM_SESSION_session_id

-- DROP INDEX public."idx_TBM_SESSION_session_id";

CREATE INDEX "idx_TBM_SESSION_session_id"
    ON public."TBM_SESSION" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;






-- Table: public.TBM_SEGMENT

-- DROP TABLE public."TBM_SEGMENT";

CREATE TABLE public."TBM_SEGMENT"
(
    session_id text COLLATE pg_catalog."default" NOT NULL,
    segment_no text COLLATE pg_catalog."default" NOT NULL,
    segment_name text COLLATE pg_catalog."default",
    start_date text COLLATE pg_catalog."default" NOT NULL,
    end_date text COLLATE pg_catalog."default" NOT NULL,
    start_time text COLLATE pg_catalog."default" NOT NULL,
    end_time text COLLATE pg_catalog."default" NOT NULL,
    start_date_time timestamp without time zone NOT NULL,
    end_date_time timestamp without time zone NOT NULL,
    venue text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_SEGMENT_pkey" PRIMARY KEY (session_id, start_date_time, end_date_time),
    CONSTRAINT "fk_TBM_SESSION_session_id" FOREIGN KEY (session_id)
        REFERENCES public."TBM_SESSION" (session_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_SEGMENT"
    OWNER to adminijoin_user;

CREATE INDEX "fki_fk_TBM_SESSION_session_id"
    ON public."TBM_SEGMENT" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;




-- Table: public.TBM_SESSION_USER

-- DROP TABLE public."TBM_SESSION_USER";

CREATE TABLE public."TBM_SESSION_USER"
(
    session_id text COLLATE pg_catalog."default" NOT NULL,
    user_id text COLLATE pg_catalog."default" NOT NULL,
    registration_status text COLLATE pg_catalog."default" NOT NULL,
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_SESSION_USER_pkey" PRIMARY KEY (session_id, user_id),
    CONSTRAINT "fk_TBM_SESSION_id" FOREIGN KEY (session_id)
        REFERENCES public."TBM_SESSION" (session_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_SESSION_USER"
    OWNER to adminijoin_user;
-- Index: fki_TBM_SESSION_USER_user_id

-- DROP INDEX public."fki_TBM_SESSION_USER_user_id";

CREATE INDEX "fki_TBM_SESSION_USER_user_id"
    ON public."TBM_SESSION_USER" USING btree
    (user_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: fki_fk_TBM_SESSION_id

-- DROP INDEX public."fki_fk_TBM_SESSION_id";

CREATE INDEX "fki_fk_TBM_SESSION_id"
    ON public."TBM_SESSION_USER" USING btree
    (session_id ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX "idx_TBM_SESSION_USER_session_user_id"
    ON public."TBM_SESSION_USER" USING btree
    (
        user_id COLLATE pg_catalog."default" ASC NULLS LAST,
        session_id COLLATE pg_catalog."default" ASC NULLS LAST
    )
    TABLESPACE pg_default;




-- Table: public.TBM_SESSION_USER_HIS

-- DROP TABLE public."TBM_SESSION_USER_HIS";

CREATE TABLE public."TBM_SESSION_USER_HIS"
(
    session_id text COLLATE pg_catalog."default" NOT NULL,
    user_id text COLLATE pg_catalog."default" NOT NULL,
    registration_status text COLLATE pg_catalog."default" NOT NULL,
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    update_by text COLLATE pg_catalog."default",
    update_datetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_SESSION_USER_HIS_pkey" PRIMARY KEY (session_id, user_id),
    CONSTRAINT "fk_TBM_SESSION_HIS_id" FOREIGN KEY (session_id)
        REFERENCES public."TBM_SESSION" (session_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_SESSION_USER_HIS"
    OWNER to adminijoin_user;
-- Index: fki_TBM_SESSION_USER_HIS_user_id

-- DROP INDEX public."fki_TBM_SESSION_USER_HIS_user_id";

CREATE INDEX "fki_TBM_SESSION_USER_HIS_user_id"
    ON public."TBM_SESSION_USER_HIS" USING btree
    (user_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: fki_fk_TBM_SESSION_HIS_id

-- DROP INDEX public."fki_fk_TBM_SESSION_HIS_id";

CREATE INDEX "fki_fk_TBM_SESSION_HIS_id"
    ON public."TBM_SESSION_USER_HIS" USING btree
    (session_id ASC NULLS LAST)
    TABLESPACE pg_default;        




-- View: public.v_Segment_Gen_Qr

-- DROP VIEW public."v_Segment_Gen_Qr";

CREATE OR REPLACE VIEW public."v_Segment_Gen_Qr"
 AS
 SELECT ss.course_id,
    ss.course_name,
    ss.course_name_th,
    ss.session_id,
    ss.session_name,
    sg.venue,
    sg.segment_name,
    to_timestamp(((to_char(sg.intervalday, 'YYYY-MM-DD'::text) || ' '::text) || to_char(sg.start_date_time, 'HH24:MI:SS'::text)), 'YYYY-MM-DD HH24:MI:SS'::text) AS start_date_time,
    to_timestamp(((to_char(sg.intervalday, 'YYYY-MM-DD'::text) || ' '::text) || to_char(sg.end_date_time, 'HH24:MI:SS'::text)), 'YYYY-MM-DD HH24:MI:SS'::text) AS end_date_time
   FROM (( SELECT "TBM_SEGMENT".session_id,
            "TBM_SEGMENT".start_date,
            "TBM_SEGMENT".end_date,
            min("TBM_SEGMENT".start_date_time) AS start_date_time,
            max("TBM_SEGMENT".end_date_time) AS end_date_time,
            max("TBM_SEGMENT".venue) AS venue,
            max("TBM_SEGMENT".segment_name) AS segment_name,
            generate_series(((min("TBM_SEGMENT".start_date_time))::date)::timestamp with time zone, ((max("TBM_SEGMENT".end_date_time))::date)::timestamp with time zone, '1 day'::interval) AS intervalday
           FROM "TBM_SEGMENT"
          GROUP BY "TBM_SEGMENT".session_id, "TBM_SEGMENT".start_date, "TBM_SEGMENT".end_date
          ORDER BY "TBM_SEGMENT".session_id, (min("TBM_SEGMENT".start_date_time))) sg
     JOIN "TBM_SESSION" ss ON ((sg.session_id = ss.session_id)));

ALTER TABLE public."v_Segment_Gen_Qr"
    OWNER TO adminijoin_user;

