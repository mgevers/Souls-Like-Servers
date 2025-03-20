import { MonsterState } from "./monsters/monster.reducer"
import { ToastState } from "./toast/toast.reducer"

export type AppState = {
    monsters: MonsterState,
    toast: ToastState,
}
