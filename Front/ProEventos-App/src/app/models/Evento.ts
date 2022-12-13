import { Lote } from "./Lote";
import { PalestranteEvento } from "./PalestranteEvento";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
    Id: number;
    Local: string;
    DataEvento?: Date;
    Tema: string;
    QtdPessoas: number;
    ImagemUrl: string;
    Telefone: string;
    Email: string;
    Lotes?: Lote[];
    RedesSocias?: RedeSocial[];
    PalestrantesEventos?: PalestranteEvento[];
}
