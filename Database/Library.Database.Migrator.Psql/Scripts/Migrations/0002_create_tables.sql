CREATE TABLE IF NOT EXISTS reservations.outbox_messages (
    id uuid NOT NULL,
    occurred_on timestamp NOT NULL,
    processed_at timestamp,
    type VARCHAR NOT NULL,
    data JSON NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.inbox_messages (
    id uuid NOT NULL,
    occurred_on timestamp NOT NULL,
    processed_at timestamp,
    type VARCHAR NOT NULL,
    data JSON NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.internal_commands (
    id uuid NOT NULL,
    created_at timestamp NOT NULL,
    processed_at timestamp,
    type VARCHAR NOT NULL,
    data JSON NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.patrons (
    id uuid NOT NULL,
    patron_type VARCHAR NOT NULL,
    version_id INTEGER NOT NULL,
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
   version_id INTEGER NOT NULL, 
   PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.holds (
    id uuid NOT NULL,
    patron_id uuid NOT NULL REFERENCES reservations.patrons (id),
    book_id uuid NOT NULL REFERENCES reservations.books (id),
    library_branch_id uuid NOT NULL,
    created_at date NOT NULL,
    till date,
    status VARCHAR NOT NULL,
    is_active BOOLEAN NOT NULL,
    version_id INTEGER NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.checkouts (
    id uuid NOT NULL,
    patron_id uuid NOT NULL REFERENCES reservations.patrons (id),
    book_id uuid NOT NULL REFERENCES reservations.books (id),
    library_branch_id uuid NOT NULL,
    due_date DATE NOT NULL, 
    version_id INTEGER NOT NULL,
    PRIMARY KEY (id)                                             
);