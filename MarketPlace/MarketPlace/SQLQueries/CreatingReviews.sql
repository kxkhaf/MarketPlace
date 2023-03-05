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