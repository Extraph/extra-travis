--dotnet tool install --global dotnet-ef

--dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=5433;Database=userijoin_database;Username=userijoin_user;Password=userijoin_password" Npgsql.EntityFrameworkCore.PostgreSQL -o EfUserModels -f

ALTER DATABASE userijoin_database SET "TimeZone" TO 'Asia/Bangkok';

CREATE TABLE public."TBM_USER_SESSION"
(
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
    CONSTRAINT "TBM_USER_SESSION_pkey" PRIMARY KEY (session_id)
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_USER_SESSION"
    OWNER to userijoin_user;

CREATE INDEX "idx_TBM_USER_SESSION_course_id"
    ON public."TBM_USER_SESSION" USING btree
    (course_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX "idx_TBM_USER_SESSION_session_id"
    ON public."TBM_USER_SESSION" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;




CREATE TABLE public."TBM_USER_SEGMENT"
(
    session_id text COLLATE pg_catalog."default" NOT NULL,
    segment_name text COLLATE pg_catalog."default",
    start_date text COLLATE pg_catalog."default" NOT NULL,
    end_date text COLLATE pg_catalog."default" NOT NULL,
    start_time text COLLATE pg_catalog."default" NOT NULL,
    end_time text COLLATE pg_catalog."default" NOT NULL,
    start_date_time timestamp without time zone NOT NULL,
    end_date_time timestamp without time zone NOT NULL,
    venue text COLLATE pg_catalog."default",
    createdatetime timestamp without time zone NOT NULL DEFAULT now(),
    CONSTRAINT "TBM_USER_SEGMENT_pkey" PRIMARY KEY (session_id, start_date_time, end_date_time),
    CONSTRAINT "fk_TBM_SESSION_session_id" FOREIGN KEY (session_id)
        REFERENCES public."TBM_USER_SESSION" (session_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_USER_SEGMENT"
    OWNER to userijoin_user;

CREATE INDEX "fki_fk_TBM_USER_SESSION_session_id"
    ON public."TBM_USER_SEGMENT" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;




CREATE TABLE public."TBM_USER_SESSION_USER"
(
    session_id text COLLATE pg_catalog."default" NOT NULL,
    user_id text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "TBM_USER_SESSION_USER_pkey" PRIMARY KEY (session_id, user_id),
    CONSTRAINT "fk_TBM_SESSION_id" FOREIGN KEY (session_id)
        REFERENCES public."TBM_USER_SESSION" (session_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."TBM_USER_SESSION_USER"
    OWNER to userijoin_user;

CREATE INDEX "fki_TBM_USER_SESSION_USER_user_id"
    ON public."TBM_USER_SESSION_USER" USING btree
    (user_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX "fki_fk_TBM_USER_SESSION_id"
    ON public."TBM_USER_SESSION_USER" USING btree
    (session_id ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX "idx_TBM_USER_SESSION_USER_session_user_id"
    ON public."TBM_USER_SESSION_USER" USING btree
    (
        user_id COLLATE pg_catalog."default" ASC NULLS LAST,
        session_id COLLATE pg_catalog."default" ASC NULLS LAST
    )
    TABLESPACE pg_default;




CREATE SEQUENCE public."TB_USER_REGISTRATION_id_seq"
    INCREMENT 1
    START 1
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."TB_USER_REGISTRATION_id_seq"
    OWNER TO userijoin_user;



CREATE TABLE public."TB_USER_REGISTRATION"
(
    id integer NOT NULL DEFAULT nextval('"TB_USER_REGISTRATION_id_seq"'::regclass),
    session_id text COLLATE pg_catalog."default" NOT NULL,
    user_id text COLLATE pg_catalog."default" NOT NULL,
    start_date_qr text COLLATE pg_catalog."default",
    is_check_in character(1) COLLATE pg_catalog."default" NOT NULL DEFAULT '0'::bpchar,
    is_check_out character(1) COLLATE pg_catalog."default" NOT NULL DEFAULT '0'::bpchar,
    check_in_datetime timestamp without time zone NOT NULL DEFAULT now(),
    check_in_date text COLLATE pg_catalog."default",
    check_in_time integer NOT NULL,
    check_out_datetime timestamp without time zone NOT NULL DEFAULT now(),
    check_out_date text COLLATE pg_catalog."default",
    check_out_time integer NOT NULL,
    check_in_by text COLLATE pg_catalog."default",
    check_out_by text COLLATE pg_catalog."default",
    CONSTRAINT "TB_USER_REGISTRATION_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public."TB_USER_REGISTRATION"
    OWNER to userijoin_user;

CREATE INDEX idx_tb_regis_checkin
    ON public."TB_USER_REGISTRATION" USING btree
    (check_in_datetime ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX idx_tb_regis_com
    ON public."TB_USER_REGISTRATION" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST, user_id COLLATE pg_catalog."default" ASC NULLS LAST, check_in_date COLLATE pg_catalog."default" ASC NULLS LAST, check_in_datetime ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX idx_tb_regis_session
    ON public."TB_USER_REGISTRATION" USING btree
    (session_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;

CREATE INDEX idx_tb_regis_user
    ON public."TB_USER_REGISTRATION" USING btree
    (user_id COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;