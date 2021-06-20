
set @dataIni = '2021-05-06';
set @dataFim = '2021-05-06';
SELECT Descricao,VARD,sigla,ABS(SUM(ff.volume)), 
Round((ABS(SUM(ff.volume))/(select sum(volume) from acoes abc where datapregao = acoes.datapregao
and abc.AcoesInfoId = acoes.AcoesInfoId)),2) as Taxa
,(select sum(volume) from acoes abc where datapregao = acoes.datapregao
and abc.AcoesInfoId = acoes.AcoesInfoId) as SomaVol, DataPregao
 FROM fluxobolsa ff inner join acoesInfo ac 
on (ff.AcoesInfoId = ac.Id )
inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
where codCorretora in('040', '016', '238', '072', '045', '013', '008') 
and data BETWEEN @dataIni and @dataFim
#and sigla = 'CEAB3'
group by  Descricao, Data,acoes.volume, vard,acoes.DataPregao
having SUM(ff.volume) < 0 
order by Descricao,DataPregao,Taxa desc


