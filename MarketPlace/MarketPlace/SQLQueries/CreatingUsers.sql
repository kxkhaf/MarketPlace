CREATE TABLE users
(
    id serial NOT NULL,
    name character varying(50) NOT NULL,
    password character varying(100) NOT NULL,
    balance BIGINT default 0,
    PRIMARY KEY (id)
);
ALTER TABLE IF EXISTS public.users
    OWNER to omr;