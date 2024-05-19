CREATE TABLE IF NOT EXISTS reservations.patrons (
    id uuid NOT NULL,
    patron_type VARCHAR NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS app.migrations_journal (
    id SERIAL NOT NULL,
    script_name VARCHAR NOT NULL,
    applied DATE NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.books (
   id uuid NOT NULL,
   library_branch_id uuid NOT NULL,
   book_category VARCHAR NOT NULL,
   PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.holds (
    id uuid NOT NULL,
    patron_id uuid NOT NULL REFERENCES reservations.patrons (id),
    book_id uuid NOT NULL REFERENCES reservations.books (id),
    library_branch_id uuid NOT NULL,
    created_at date NOT NULL,
    period VARCHAR NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.patron_hold_decisions (
    id uuid NOT NULL,
    hold_id uuid NOT NULL REFERENCES reservations.holds (id),
    decision_status VARCHAR,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS  reservations.library_hold_decisions (
    id uuid NOT NULL,
    hold_id uuid NOT NULL REFERENCES reservations.holds (id),
    decision_status VARCHAR,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.checkouts (
    id uuid NOT NULL,
    patron_id uuid NOT NULL REFERENCES reservations.patrons (id),
    book_id uuid NOT NULL REFERENCES reservations.books (id),
    library_branch_id uuid NOT NULL,
    due_date DATE NOT NULL, 
    PRIMARY KEY (id)                                             
)