CREATE TABLE IF NOT EXISTS reservations.patrons (
    Id uuid NOT NULL,
    PatronType VARCHAR NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IF NOT EXISTS app.migrations_journal (
    Id SERIAL NOT NULL,
    ScriptName VARCHAR(250) NOT NULL,
    Applied DATE NOT NULL,
    PRIMARY KEY (Id)
);
