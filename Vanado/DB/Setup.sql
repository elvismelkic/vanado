DROP TABLE IF EXISTS files;
DROP TABLE IF EXISTS failures;
DROP TABLE IF EXISTS machines;
DROP TYPE IF EXISTS priority;

CREATE TABLE machines (
  id serial PRIMARY KEY,
  name text NOT NULL UNIQUE,
  CONSTRAINT name_not_empty
    CHECK (char_length(name) > 0)
);

CREATE TYPE priority AS ENUM ('low', 'moderate', 'high');

CREATE TABLE failures (
  id serial PRIMARY KEY,
  name text NOT NULL,
  description text,
  priority priority NOT NULL,
  is_fixed bool NOT NULL,
  machine_id int REFERENCES machines(id) ON DELETE CASCADE NOT NULL,
  inserted_at timestamp NOT NULL,
  updated_at timestamp NOT NULL,
  CONSTRAINT name_not_empty
    CHECK (char_length(name) > 0),
  CONSTRAINT name_longer_than_20_for_description
    CHECK (char_length(name) > 20 OR description IS NULL)
);

CREATE INDEX failure_idx_machine ON failures (machine_id);

CREATE TABLE files (
  id serial PRIMARY KEY,
  name text NOT NULL,
  type text NOT NULL,
  failure_id int REFERENCES failures(id) ON DELETE CASCADE NOT NULL,
  UNIQUE (name, failure_id)
);

CREATE INDEX failure_idx_file ON files (failure_id);

INSERT INTO machines (name) VALUES ('Machine 1');
INSERT INTO machines (name) VALUES ('Machine 2');
INSERT INTO machines (name) VALUES ('Machine 3');

-- Failure #1
INSERT INTO failures
  (name, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 1', 'low', false, 1, '2020-09-05 04:05:06', '2020-09-05 04:05:06');

-- Failure #2
INSERT INTO failures
  (name, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 2', 'high', false, 1, '2020-09-05 04:15:06', '2020-09-05 04:15:06');

-- Failure #3
INSERT INTO failures
  (name, description, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 3 with extra long name', 'Description of failure 3', 'high', false, 1, '2020-09-05 03:05:06', '2020-09-05 03:05:06');

-- Failure #4
INSERT INTO failures
  (name, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 4', 'moderate', false, 3, '2020-09-05 04:05:06', '2020-09-05 04:05:06');

-- Failure #5
INSERT INTO failures
  (name, description, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 5 with extra long name', 'Description of failure 5', 'high', false, 3, '2020-09-05 03:35:06', '2020-09-05 03:35:06');

-- Failure #6
INSERT INTO failures
  (name, description, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 6 with extra long name', 'Description of failure 6', 'low', true, 3, '2020-09-05 05:05:06', '2020-09-05 05:05:06');

-- Failure #7
INSERT INTO failures
  (name, priority, is_fixed, machine_id, inserted_at, updated_at)
VALUES
  ('Failure 7', 'high', false, 3, '2020-09-05 06:05:06', '2020-09-05 06:05:06');
