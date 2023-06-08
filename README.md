# SleekFlow

## Setting Database - Postgres

1. Download Postgres
2. Create database "postgres"
3. Create Tables:
CREATE TABLE IF NOT EXISTS public.status
(
    status_id integer NOT NULL DEFAULT nextval('status_status_id_seq'::regclass),
    status_name character varying(50) COLLATE pg_catalog."default"
)

CREATE TABLE IF NOT EXISTS public.tag
(
    tag_id integer NOT NULL DEFAULT nextval('tag_tag_id_seq'::regclass),
    tag_name character varying(50) COLLATE pg_catalog."default"
)

CREATE TABLE IF NOT EXISTS public.todo_tag_xref
(
    todo_id integer,
    tag_id integer
)

CREATE TABLE IF NOT EXISTS public.todos
(
    todo_id integer NOT NULL DEFAULT nextval('todos_todo_id_seq'::regclass),
    todo_name character varying(50) COLLATE pg_catalog."default",
    todo_description character varying(500) COLLATE pg_catalog."default",
    todo_due_date date,
    todo_status character varying(50) COLLATE pg_catalog."default"
)

## Start Backend Server (.Net Core Web API)
1. Open with Visual Studio
2. Start IIS Express

## Start Frontend Server (UI)
1. cd into './SleekFlowUI'
2. run npm install
3. run ng serve