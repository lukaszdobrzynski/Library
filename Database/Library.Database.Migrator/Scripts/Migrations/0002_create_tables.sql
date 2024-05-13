CREATE TABLE IF NOT EXISTS reservations.patrons (
    id uuid NOT NULL,
    patron_type VARCHAR NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS app.migrations_journal (
    id SERIAL NOT NULL,
    script_name VARCHAR(250) NOT NULL,
    applied DATE NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS reservations.holds (
    id uuid NOT NULL,
    bookId uuid NOT NULL,
    status VARCHAR NOT NULL,
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
