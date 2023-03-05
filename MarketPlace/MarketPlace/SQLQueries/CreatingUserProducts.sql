CREATE TABLE public.user_products
(
    user_id int NOT NULL,
    product_id int NOT NULL,
    product_count BIGINT default 0,
    PRIMARY KEY (user_id, product_id)
);

ALTER TABLE IF EXISTS public.user_products
    OWNER to omr;