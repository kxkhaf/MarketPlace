CREATE TABLE public.products
(
    id serial NOT NULL,
    name character varying(50) NOT NULL,
    information text,
    rating numeric default -1,
    price numeric not null,
    PRIMARY KEY (id)
);

ALTER TABLE IF EXISTS public.products
    OWNER to omr;
CREATE TABLE public.reviews
(
    id serial NOT NULL,
    reviewer_id int NOT NULL,
    product_id int NOT NULL,
    rating int default -1,
    message text,
    PRIMARY KEY (id)
);

ALTER TABLE IF EXISTS public.reviews
    OWNER to omr;
CREATE TABLE public.user_products
(
    user_id int NOT NULL,
    product_id int NOT NULL,
    product_count BIGINT default 0,
    PRIMARY KEY (user_id, product_id)
);

ALTER TABLE IF EXISTS public.user_products
    OWNER to omr;

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