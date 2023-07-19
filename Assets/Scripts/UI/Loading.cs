using UnityEditor;
using System.Threading.Tasks;

namespace UI {
    public static class Loading {
        public delegate void BeforeLoadingDelegate();
        
        public static BeforeLoadingDelegate BeforeLoading;

        // public static void BeginLoading() {
        //     if (BeforeLoading != null) {
        //         BeforeLoading().then(Loading());
        //     }
        //
        //     Loading();
        // }
        //
        // public static async Task BeginLoadingAsync() {
        //     if (BeforeLoading != null) {
        //         await BeforeLoading();
        //     }
        //     
        //     await LoadContentAsync();
        // }
        //
        //
        // private static Task Loading() {
        //     return Task.CompletedTask;
        //     // Do the loading
        // }
    }
}