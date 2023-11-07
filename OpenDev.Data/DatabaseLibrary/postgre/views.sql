--CREATE or replace VIEW core.v_column_list 
--AS
--select 
--	ca.name || '__' || s."table_name" || '__' || s."column_name" as "app_column_key",
--	ca.app_key,
--	"table_schema",
--	"table_name",
--	"column_name",ordinal_position,column_default,data_type,character_maximum_length,udt_name,is_identity,identity_generation
--from information_schema."columns" s
--join core.app ca on ca.app_key= s.table_schema
--WHERE table_schema not in 
--('information_schema','pg_catalog');



--create or replace view v_foreign_column
--as
--select 
--	cu.constraint_name,
--	cu.table_name,
--	cu.column_name,
--	kc.table_name as primary_column_name,
--	kc.column_name as foreign_column_name
--from information_schema.constraint_column_usage cu
--join information_schema.key_column_usage kc on kc.constraint_name = cu.constraint_name
--join information_schema.constraint_table_usage tu on tu.constraint_name = cu.constraint_name
--where cu.table_schema not in 
--('information_schema','pg_catalog')
--and 
--(cu.table_name::text = kc.table_name::text and cu.column_name=kc.column_name)=false;


--create or replace view core.v_primary_column
--as
--select  
--	tc."constraint_schema" as "app_key",
--	tc.table_catalog as db_name,
--	tc."table_name",
--	cu.column_name
--	--,*
--from information_schema.table_constraints tc
--join information_schema.constraint_column_usage cu on cu.constraint_name = tc.constraint_name and cu.constraint_schema = tc.constraint_schema
--where tc.table_schema not in ('pg_catalog')
--and constraint_type='PRIMARY KEY';



--create or replace view core.v_table_list
--as
--select 
--	distinct
--	"table_name",
--	"table_schema" as "schema_name"
--from information_schema.tables
--where table_schema not in ('information_schema','pg_catalog')
--and "table_name" not like 'v_%';



--create or replace view core.v_crud
--as
--select 
--	tbl.table_name,
--	fo.form_key
--from core.form_type ft
--join core.form fo on fo.form_type_key = ft.form_type_key
--join information_schema.tables tbl on ft.form_type_key='crud'
--where tbl.table_schema not in ('information_schema','pg_catalog');


	
--create or replace function core.parse_html(html text)
--returns text
--language plpgsql
--as
--$$
--declare
--	ret text := html;
--begin
--	ret := replace(ret,'<','&lt;');
--	ret := replace(ret,'>','&gt;');
	
--	return ret;
--end
--$$


--CREATE or replace VIEW core.v_field_type 
--AS
--select 
--	distinct
--	udt_name as "field_type"		
--from information_schema."columns" s
--join core.app ca on ca.app_key= s.table_schema
--WHERE table_schema not in 
--('information_schema','pg_catalog');
